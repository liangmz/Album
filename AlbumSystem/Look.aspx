<%@ Page Title="" Language="C#" MasterPageFile="~/HD.master" AutoEventWireup="true" CodeFile="Look.aspx.cs" Inherits="Look" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/look.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BODY" runat="Server">
    <div class="row">
        <div class="col-lg-4" id="LA"></div>
        <div class="col-lg-4" id="LB"></div>
        <div class="col-lg-4" id="LC"></div>
    </div>

    <div class="modal fade" id="UnLook" tabindex="-1" role="dialog" aria-labelledby="UnLookLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <br />
                    <br />
                    <center>
                       <span class="glyphicon glyphicon-question-sign"></span>
                       <h4>确定取消关注?</h4>
                       </center>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" onclick="unlookActive();">确定</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>

                </div>
            </div>
        </div>
    </div>

    <script src="js/look.js"></script>
    <script type="text/javascript">
        document.getElementById("HC").className = "active";
    </script>
</asp:Content>

