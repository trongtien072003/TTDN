﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using AutoMapper.Internal.Mappers;
using Microsoft.EntityFrameworkCore;
using proj_tt.Order.Dto;

namespace proj_tt.Order
{
    public class OrderAppService : proj_ttAppServiceBase, IOrderAppService
    {
        private readonly IRepository<Orders, long> _orderRepository;
        private readonly IRepository<OrderItem, long> _orderItemRepository;

        public OrderAppService(
            IRepository<Orders, long> orderRepository,
            IRepository<OrderItem, long> orderItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public async Task<OrderDto> GetOrder(long id)
        {
            var order = await _orderRepository.GetAll()
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            return ObjectMapper.Map<OrderDto>(order);
        }

        public async Task<PagedResultDto<OrderDto>> GetAllOrders(PagedOrderResultRequestDto input)
        {
            var query = _orderRepository.GetAll()
                .Include(o => o.OrderItems)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Keyword),
                    o => o.UserName.Contains(input.Keyword) ||
                         o.UserEmail.Contains(input.Keyword))
                .WhereIf(input.Status.HasValue, o => o.Status == input.Status.Value)
                .WhereIf(input.UserId.HasValue, o => o.UserId == input.UserId.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(o => o.CreationTime)
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<OrderDto>(
                totalCount,
                ObjectMapper.Map<List<OrderDto>>(items)
            );
        }

        public async Task UpdateOrderStatus(UpdateOrderStatusInput input)
        {
            var order = await _orderRepository.GetAsync(input.Id);
            if (order == null)
            {
                throw new Exception("Order not found");
            }

            order.Status = input.Status;
            order.Note = input.Note;

            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteOrder(long id)
        {
            var order = await _orderRepository.GetAll()
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            foreach (var item in order.OrderItems)
            {
                await _orderItemRepository.DeleteAsync(item);
            }

            await _orderRepository.DeleteAsync(order);
        }
    }
}
