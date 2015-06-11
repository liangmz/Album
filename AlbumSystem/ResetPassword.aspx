<%@ Page Title="" Language="C#" MasterPageFile="~/HD.master" AutoEventWireup="true" CodeFile="ResetPassword.aspx.cs" Inherits="PasswordReset" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BODY" runat="Server">
    
    <div id="D_resetPsw" runat="server">
        <form role="form" class="form-horizontal" method="post" runat="server">
            <div class="form-group">
                <label class="col-sm-2 control-label"></label>
                <div class="col-sm-3">
                    <h4>密码重设</h4>
                </div>
            </div>
            <div class="form-group">
                <label for="UserName" class="col-sm-2 control-label">用户名</label>
                <div class="col-sm-3">
                    <input type="text" class="form-control" id="UserName" name="UserName" placeholder="UserName" runat="server" disabled>
                </div>
            </div>
            <div class="form-group">
                <label for="Password" class="col-sm-2 control-label">新密码</label>
                <div class="col-sm-3">
                    <input type="password" class="form-control" id="Password" name="Password" placeholder="Password" required>
                </div>
            </div>
            <div class="form-group">
                <label for="r_Password" class="col-sm-2 control-label">密码确认</label>
                <div class="col-sm-3">
                    <input type="password" class="form-control" id="r_Password" name="r_Password" placeholder="Password" required>
                </div>
            </div>
            <div class="form-group">
                <label for="ResetPsw" class="col-sm-2 control-label"></label>
                <div class="col-sm-3">
                    <button id="ResetPsw" class="btn btn-primary" type="submit" runat="server" onserverclick="ResetPsw_ServerClick">提交</button>
                </div>
            </div>
            <div class="form-group">
                <label for="call" class="col-sm-2 control-label"></label>
                <div class="col-sm-3">
                    <p id="ResetReportMessage" runat="server"></p>
                </div>
            </div>
        </form>
    </div>
</asp:Content>

