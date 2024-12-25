# DiscusSDK

#### 介绍
​	DiscusSDK是一套net 8微服务架构，能满足大多数业务场景需求，当前项目不包含任何业务，您可以充分应用与您所在行业的业务。持续更新，做到高可用。感谢您的支持！本项目采用集群化部署，单机项目https://gitee.com/xuscxoudo/given-pick

主要参考框架

​	ADNC https://github.com/AlphaYu/adnc

​	老张的哲学https://github.com/BaseCoreVueProject/Blog.Core

​	大内老A https://www.cnblogs.com/artech/p/inside-asp-net-core-6.html

#### 软件架构



#### 安装教程

| 离线安装docker，上传镜像包 | https://gitee.com/xuscxoudo/deploy-docker-compose            |
| -------------------------- | ------------------------------------------------------------ |
| 镜像包地址                 | https://pan.xunlei.com/s/VOAI3OdBpPx0tQ7YnYrtEExNA1?pwd=xivp |
| 使用yml快速部署            | https://gitee.com/xuscxoudo/discus-sdk-deploy                |

#### 项目结构图



#### 技术栈

| 名称       | 状态   | 描述                                                         | 地址                                                      |
| ---------- | ------ | ------------------------------------------------------------ | --------------------------------------------------------- |
| nacos      |        | 服务发现中心，配置中心                                       | https://nacos.io/                                         |
| jwt        |        | 服务鉴权授权                                                 |                                                           |
| redis      |        | csredis集成redis缓存中间件                                   |                                                           |
| 分布式Id   |        | 雪花算法生成分布式id                                         |                                                           |
| rabbitmq   |        | dotnet.cap集成rabbitmq消息队列，作用：限流，业务隔离通信机制（ 传统开发中，基础功能服务可能需要调用最上层的业务，在上层业务服务中，订阅上传文件事件） |                                                           |
| mysql      |        | sqlsugar集成mysql，读写分离                                  | https://www.donet5.com/home/doc                           |
| minio      |        | 分布式文件库                                                 | https://min.io/download?license=enterprise&platform=linux |
| Serilog    |        | 日志工具，异常日志写入mysql数据库                            |                                                           |
| AutoMapper |        | 实体数据映射关系                                             |                                                           |
| Knife4UI   |        | 接口文档生成工具                                             |                                                           |
| Polly      |        | 弹性和瞬态故障处理                                           |                                                           |
| Ocelot     |        | 网关，路由转发                                               | https://github.com/softlgl/Ocelot.Provider.Nacos          |
| Skywalking | 未完成 |                                                              |                                                           |
| ES         |        | 分布式搜索                                                   | https://github.com/elastic/elasticsearch-net              |
| ELK        |        | 日志收集                                                     | https://www.elastic.co/cn/                                |
| httpclient |        | 服务之间通信，使用http请求                                   |  还存在bug                                                         |
| XxlJob     | | 后续会根据java执行器迁移过来                                                             | https://github.com/xuyuadmin/xxljob                       |

#### 其他