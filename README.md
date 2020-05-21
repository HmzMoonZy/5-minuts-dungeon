5-minuts-dungeon
# **简介**
**个人用于练习Socket编程及与Unity交互的案例:**  
主要实现: 
+ 通用的服务器类库
+ 针对该客户端的服务器实现
+ 客户端与服务器交互逻辑

# **目前进度**
>## ***2020年5月21日***   
>### 完成了**服务端通用类库**的逻辑。
>+ 服务器使用异步的方式监听端口
>+ 服务器使用异步的方式收发数据
>+ 规定序列化的虚方法
>+ 声明和调用继承该类库的接口

>### 完成了**服务端与客户端游戏外交互**的逻辑。
>+ 规定了协议格式.
>+ 实现接口的功能(消息转发给逻辑层)
>+ 实现代码分离的UI界面
>+ 实现与数据库的简单交互
>+ 实现用户登录及注册的流程.
>+ 实现大厅及房间系统的基本逻辑(创建房间\进入房间\退出房间).

# **效果展示**
>![logic](https://github.com/HmzMoonZy/5-minuts-dungeon/blob/master/Client/Assets/documents/%E5%88%9B%E5%BB%BA%E6%88%BF%E9%97%B4.gif)
>![logic](https://github.com/HmzMoonZy/5-minuts-dungeon/blob/master/Client/Assets/documents/%E8%BF%9B%E5%85%A5%E6%88%BF%E9%97%B4.gif)
# **实现**
>![logic](https://github.com/HmzMoonZy/5-minuts-dungeon/blob/master/Client/Assets/documents/%E5%BA%94%E7%94%A8%E5%B1%82.png)