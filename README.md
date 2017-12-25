Hangfire的控制台程序示例。

# Hangfire简介

来源：http://www.cnblogs.com/redmoon/p/4394962.html

Hangfire是一个开源且商业免费使用的工具函数库。可以让你非常容易地在ASP.NET应用（也可以不在ASP.NET应用）中执行多种类型的后台任务，而无需自行定制开发和管理基于Windows Service后台任务执行器。且任务信息可以被持久保存。内置提供集成化的控制台。

通过Nuget就可以在你的应用程序中安装Hangfire：`Install-Package Hangfire`

Hangfire的具有如下特性和有点：

- 支持基于队列的任务处理：任务执行不是同步的，而是放到一个持久化队列中，以便马上把请求控制权返回给调用者。使用方法：`BackgroundJob.Enqueue(() => Console.WriteLine("Simple!"));`
- 延迟任务执行：不是马上调用方法，而是设定一个未来时间点再来执行。使用方法：`BackgroundJob.Schedule(() => Console.WriteLine("Reliable!"), TimeSpan.FromDays(7));`
- 循环任务执行：只需要简单的一行代码就可以添加重复执行的任务，其内置了常见的时间循环模式，也可以基于CRON表达式来设定复杂的模式。使用方法：`RecurringJob.AddOrUpdate(() => Console.WriteLine("Transparent!"), Cron.Daily);`
- 持久化保存任务、队列、统计信息：默认使用SQL Server，也可以配合消息队列来降低队列处理延迟，或配置使用Redis来获得更好的性能表现
- 内置自动重试机制：可以设定重试次数，还可以手动在控制台重启任务
- 除了调用静态方法外还支持实例方法
- 能够捕获多语言状态：即可以把调用者的`Thread.CurrentCulture`和`Thread.CurrentUICulture`信息同任务持久保存在一起，以便任务执行的时候多语言信息是一致的
- 支持任务取消：使用`CancellationToken`这样的机制来处理任务取消逻辑
- 支持IoC容器：目前支持`Ninject`和`Autofac`比较常用的开源IoC容器
- 支持Web集群：可以在一台或多台机器上运行多个Hangfire实例以便实现冗余备份
- 支持多队列：同一个Hangfire实例可以支持多个队列，以便更好的控制任务的执行方式
- 并发级别的控制：默认是处理器数量的5倍工作行程，当然也可以自己设定
- 具备很好的扩展性：有很多扩展点来控制持久存储方式、IoC容器支持等

为什么要使用Hangfire这样的函数库呢？我觉得好处有如下几个方面：

1. 开发简单：无需自己额外做开发，就可以实现任务的队列执行、延迟执行和重复执行
2. 部署简单：可以同主ASP.NET应用部署在一起，测试和维护都相对简单
3. 迁移简单：由于宿主不仅限于ASP.NET，那么未来可以非常容易的把任务执行器放到其他地方（需要改变的就是在其他宿主中启动Hangfire服务器）
4. 扩展简单：由于开源且有很多扩展点，在现有插件都不满足自己需要的情况下能够容易的进行扩展

之前我把Hangfire运用到两种情况下：

1. 后台长时间的科学计算：这样就可以让请求马上返回给客户端，后台完成长时间计算后，用SignalR实时提醒用户
2. 后台群发邮件：通过延迟和循环任务分批通过SendCloud这样的服务发送群发邮件

当然，Hangfire的应用场景还很多，比如在后台处理电商卖家的订单。