using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Timing;
using Abp.UI;
using AutoMapper.Internal.Mappers;
using Microsoft.EntityFrameworkCore;
using MiniExcelLibs;
using proj_tt.Order.Dto;
using System.Linq.Dynamic.Core;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.ObjectMapping;

namespace proj_tt.Order
{
    [AbpAuthorize("Pages.Orders.Admin")]
    public class OrderAppService : ApplicationService, IOrderAppService
    {
        private readonly IRepository<Orders, long> _orderRepository;
        private readonly IRepository<OrderItem, long> _orderItemRepository;

        public OrderAppService(IRepository<Orders, long> orderRepository, IRepository<OrderItem, long> orderItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        /// <summary>
        /// Lấy chi tiết một đơn hàng bất kỳ theo ID (không cần check UserId)
        /// </summary>
        [AbpAuthorize("Pages.Orders.View")]
        public async Task<OrderDto> GetOrderAsync(long id)
        {
            var order = await _orderRepository.GetAllIncluding(o => o.OrderItems)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                throw new UserFriendlyException("Không tìm thấy đơn hàng.");

            return ObjectMapper.Map<OrderDto>(order);
        }

        /// <summary>
        /// Lấy danh sách tất cả đơn hàng có hỗ trợ tìm kiếm, lọc và phân trang, bao gồm lọc theo khoảng ngày tạo
        /// </summary>
        [AbpAuthorize("Pages.Orders.View")]
        public async Task<PagedResultDto<OrderDto>> GetAllAsync(PagedOrderRequestDto input)
        {
            var query = _orderRepository.GetAll()
                .Include(o => o.OrderItems)
                .AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Keyword),
                    o => o.UserName.Contains(input.Keyword) || o.UserEmail.Contains(input.Keyword))
                .WhereIf(input.Status.HasValue, o => o.Status == input.Status)
                .WhereIf(input.MinDate.HasValue, o => o.CreationTime >= input.MinDate.Value)
                .WhereIf(input.MaxDate.HasValue, o => o.CreationTime <= input.MaxDate.Value)
                .WhereIf(input.MinTotal.HasValue, o => o.TotalAmount >= input.MinTotal.Value)
                .WhereIf(input.MaxTotal.HasValue, o => o.TotalAmount <= input.MaxTotal.Value);

            var totalCount = await query.CountAsync();

            var orders = await query
                .OrderBy(input.Sorting ?? "CreationTime DESC")
            .PageBy(input)
            .ToListAsync();

            return new PagedResultDto<OrderDto>(totalCount, ObjectMapper.Map<List<OrderDto>>(orders));
        }

        /// <summary>
        /// Cập nhật trạng thái và ghi chú của đơn hàng
        /// </summary>
        [AbpAuthorize("Pages.Orders.Update")]
        public async Task UpdateStatusAsync(UpdateOrderStatusInput input)
        {
            var order = await _orderRepository.GetAsync(input.Id);
            order.Status = (OrderStatus)input.Status;
            order.Note = input.Note?.Trim();
            order.LastModificationTime = Clock.Now;
            await _orderRepository.UpdateAsync(order);
        }

        /// <summary>
        /// Xóa mềm đơn hàng và các item đi kèm
        /// </summary>
        [AbpAuthorize("Pages.Orders.Delete")]
        public async Task DeleteAsync(long id)
        {
            var order = await _orderRepository.GetAllIncluding(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                throw new UserFriendlyException("Không tìm thấy đơn hàng.");

            foreach (var item in order.OrderItems)
            {
                await _orderItemRepository.DeleteAsync(item);
            }

            await _orderRepository.DeleteAsync(order);
        }

        /// <summary>
        /// Phục hồi đơn hàng và các item từ trạng thái bị xóa mềm
        /// </summary>
        [AbpAuthorize("Pages.Orders.Restore")]
        public async Task RestoreAsync(long id)
        {
            var order = await _orderRepository.GetAllIncluding(o => o.OrderItems)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(o => o.Id == id && o.IsDeleted);

            if (order == null)
                throw new UserFriendlyException("Không tìm thấy đơn hàng đã bị xóa.");

            order.IsDeleted = false;
            order.DeletionTime = null;
            order.DeleterUserId = null;

            foreach (var item in order.OrderItems)
            {
                item.IsDeleted = false;
                item.DeletionTime = null;
                item.DeleterUserId = null;
                await _orderItemRepository.UpdateAsync(item);
            }

            await _orderRepository.UpdateAsync(order);
        }

        /// <summary>
        /// Xuất toàn bộ đơn hàng và chi tiết sản phẩm ra file Excel (2 sheet)
        /// </summary>
        [AbpAuthorize("Pages.Orders.Export")]
        public async Task<FileDto> ExportAllToFileAsync()
        {
            var orders = await _orderRepository.GetAll()
                .Include(o => o.OrderItems)
                .AsNoTracking()
            .OrderByDescending(o => o.CreationTime)
                .ToListAsync();

            var dtos = ObjectMapper.Map<List<OrderDto>>(orders);

            var sheet1 = dtos.Select(o => new
            {
                o.Id,
                o.UserName,
                o.UserEmail,
                o.PhoneNumber,
                o.Address,
                TotalAmount = o.TotalAmount.ToString("N0"),
                Status = o.Status.ToString(),
                o.Note,
                CreationTime = o.CreationTime.ToString("yyyy-MM-dd HH:mm"),
                LastModified = o.LastModificationTime?.ToString("yyyy-MM-dd HH:mm")
            });

            var sheet2 = dtos.Where(o => o.OrderItems != null)
                .SelectMany(o => o.OrderItems.Select(i => new
                {
                    OrderId = o.Id,
                    i.ProductId,
                    i.ProductName,
                    i.Quantity,
                    Price = i.Price.ToString("N0"),
                    TotalPrice = i.TotalPrice.ToString("N0"),
                    CreationTime = i.CreationTime.ToString("yyyy-MM-dd HH:mm"),
                    LastModified = i.LastModificationTime?.ToString("yyyy-MM-dd HH:mm")
                }));

            using var stream = new MemoryStream();
            await stream.SaveAsAsync(new Dictionary<string, object>
            {
                ["Orders"] = sheet1.ToList(),
                ["OrderItems"] = sheet2.ToList()
            });

            return new FileDto
            {
                FileName = $"Orders_{DateTime.Now:yyyyMMddHHmmss}.xlsx",
                FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileBytes = stream.ToArray()
            };
        }
    }
}
