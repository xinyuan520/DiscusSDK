﻿using Discus.SDK.RabbitMQ.Consts;
using Discus.SDK.Repository.SqlSugar.Repository;
using Discus.User.Repository.Entities;
using DotNetCore.CAP;

namespace Discus.User.Application.Services
{
    public class MessageService : BasicService, IMessageService
    {
        private readonly ICapPublisher _capPublisher;
        private readonly IBaseRepository<UserInfo> _userinfoRepository;


        public MessageService(ICapPublisher capPublisher, IBaseRepository<UserInfo> userinfoRepository)
        {
            _capPublisher = capPublisher;
            _userinfoRepository = userinfoRepository;
        }

        public async Task Publish() 
        {
            var userInfo = await _userinfoRepository.GetByIdAsync(1);
            await _capPublisher.PublishAsync(EventNameConsts.DiscusRole, userInfo);
            // 使用 direct 交换机发布消息
            //var directHeaders = new Dictionary<string, string>
            //                    {
            //                        { "cap.exchangeType", "direct" }
            //                    };
            //await _capPublisher.PublishAsync(EventNameConsts.DiscusUser, userInfo, directHeaders);
        }
    }
}
