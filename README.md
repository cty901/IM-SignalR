# IM-SignalR
https://www.cnblogs.com/zhangjun0204/p/11999912.html


.Net core 3.0 SignalR+Vue 实现简单的即时通讯/聊天IM （无jq依赖）
.Net core 中的SignalR JavaScript客户端已经不需要依赖Jquery了

一、服务端
1、nuget安装 Microsoft.AspNetCore.SignalR
2、在startup.cs中注册和使用signalr

            services.AddSignalR();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chatHub");
                endpoints.MapControllers();
            });
3、创建chathub类并继承Hub

public class ChatHub:Hub
    {
        /// <summary>
        /// 给连接的所有人发送消息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendMsg(string username,string message)
        {
             //Show方法需要在前端实现
            return Clients.All.SendAsync("Show", username , message);
        }
    }
二、客户端
客户端用vue实现
1、安装signalr.js

npm install @microsoft/signalr
<template>
  <div class="hello">
    <el-input v-model="user" type="text" />
    <div id="message" v-html="remsg"></div>
    <div id="el-input">
    <el-input id="chatbox" @keyup.native.enter="handle"  type="textarea" :rows="1" placeholder="请输入内容" v-model="msg"></el-input>

    </div>
    <el-button size="small" style="display:inline-block;" icon="el-icon-s-promotion" type="suceess" @click="handle" plain></el-button>
  </div>
</template>

<script>
import * as signalR from "@microsoft/signalr";
let hubUrl = "http://localhost:5001/chatHub";
//.net core 版本中默认不会自动重连，需手动调用 withAutomaticReconnect 
const connection = new signalR.HubConnectionBuilder().withAutomaticReconnect().withUrl(hubUrl).build();
connection.start().catch(err => alert(err.message));
export default {
  name: "Im",

  mounted() {
    var _this = this;
    //实现Show方法
    connection.on("Show", function(username, message) {
      _this.remsg = _this.remsg + "<br>" + username + ":" + message;
    });
  },
  data() {
    return {
      user: "",
      msg: "",
      remsg: ""
    };
  },

  methods: {
    handle: function() {
      if(this.msg.trim()==""){
        alert("不能发送空白消息");
        return;
      }
      //调用后端方法 SendMsg 传入参数
      connection.invoke("SendMsg", this.user, this.msg);
      this.msg = "";
    }
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h1,
h2 {
  font-weight: normal;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
#el-input{
  display: inline-block;
  width:96%;
  float: left;
}
#message {

  overflow-y:auto;
  text-align: left;
  border: #42b983 solid 1px;
  height: 500px;

}

</style>
