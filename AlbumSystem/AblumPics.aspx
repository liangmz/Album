<%@ Page Title="" Language="C#" MasterPageFile="~/HD.master" AutoEventWireup="true" CodeFile="AblumPics.aspx.cs" Inherits="AblumPics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/myPic.js"></script>
    <script src="js/ablumpic.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BODY" runat="Server">
    <div class="alert alert-info">
        <button type="button" id="UP" class="btn btn-primary" data-toggle="modal" data-target="#Upload">上传图片</button>
        <button type="button" id="SelectAll" class="btn btn-primary" onclick="SelectAllCheckBox();">全部选中</button>
        <button type="button" id="SelectUn" class="btn btn-primary" onclick="SelectUnCheckBox();" >全部反选</button>
        <button type="button" id="Delete" class="btn btn-primary" data-toggle="modal" data-target="">选中删除</button>
    </div>
    <ol class="breadcrumb">
        <li><a href="MyAblum.aspx">Ablum</a></li>
        <li class="active" id="AblumName"></li>
    </ol>

    <div class="col-lg-2" id="PA"></div>
    <div class="col-lg-2" id="PB"></div>
    <div class="col-lg-2" id="PC"></div>
    <div class="col-lg-2" id="PD"></div>
    <div class="col-lg-2" id="PE"></div>
    <div class="col-lg-2" id="PF"></div>

    <div class="modal fade" id="Upload" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="myModalLabel">图片上传</h4>
                </div>
                <div class="modal-body">
                    <form id="uploadPictures" class="form-horizontal" role="form">
                        <div class="form-group">
                            <div class="col-sm-5">
                                <input id="p" type="file" required multiple="multiple" accept="image/*" />
                            </div>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" onclick="uploadPic()">Upload</button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>

            </div>
        </div>
    </div>
</asp:Content>

