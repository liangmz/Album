﻿<%@ Page Title="" Language="C#" MasterPageFile="~/HD.master" AutoEventWireup="true" CodeFile="Infomation.aspx.cs" Inherits="Infomation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BODY" runat="Server">
    <div>
        <h2><img src="#" style="width:64px;height:64px;" id="mypic" runat="server"> 我的信息</h2>
    </div>
    
    <div class="col-sm-3">
        <form class="form-horizontal" method="post" enctype="multipart/form-data">
            <div class="form-group">
                <label for="info_old_password" class=" control-label">旧密码</label>
                <input type="password" class="form-control" id="info_old_password" name="info_old_password" placeholder="******" required autofocus />
            </div>
            <div class="form-group">
                <label for="info_new_password" class=" control-label">新密码</label>
                <input type="password" class="form-control" id="info_new_password" name="info_new_password" />
            </div>
            <div class="form-group">
                <label for="info_new_pic" class=" control-label">更新头像</label>
                <input type="file" class="form-control" id="info_new_pic" name="info_new_pic" accept="image/*" />
            </div>
            <div class="form-group">
                <label for="info_newEmail" class=" control-label">邮箱</label>
                <input type="email" class="form-control" id="info_newEmail" name="info_newEmail" placeholder="<%=Session["email"] == null ? "" : Session["email"].ToString() %>" />
            </div>
            <div class="form-group">
                <button type="submit" class="btn btn-primary">更新</button>
                <a id="errorInfo" runat="server"></a>
            </div>
        </form>
    </div>
    <script type="text/javascript">
        window.onload = function () { };
    </script>
</asp:Content>

