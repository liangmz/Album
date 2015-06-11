<%@ Page Title="" Language="C#" MasterPageFile="~/HD.master" AutoEventWireup="true" CodeFile="MyAblum.aspx.cs" Inherits="MyAblum" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/ablum.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BODY" runat="Server">
    <div class="alert alert-info">
        <button type="button" id="CA" class="btn btn-primary" data-toggle="modal" data-target="#Create">创建专辑</button>
    </div>
    <ol class="breadcrumb">
        <li class="active">Ablum</li>
    </ol>
    <div class="row">
        <div class="col-lg-4" id="AA">
            <div class="thumbnail" id="LA">
                <span class="glyphicon glyphicon-refresh tagLoading"></span>Loading...
            </div>
        </div>
        <div class="col-lg-4" id="AB">
            <div class="thumbnail" id="LB">
                <span class="glyphicon glyphicon-refresh tagLoading"></span>Loading...
            </div>
        </div>
        <div class="col-lg-4" id="AC">
            <div class="thumbnail" id="LC">
                <span class="glyphicon glyphicon-refresh tagLoading"></span>Loading...
            </div>
        </div>
    </div>

    <div class="modal fade" id="Create" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="myModalLabel">创建专辑</h4>
                </div>
                <div class="modal-body">

                    <form class="form-horizontal" role="form">
                        <div class="form-group">
                            <label for="inputEmail3" class="col-sm-3 control-label">专辑名</label>
                            <div class="col-sm-6">
                                <input type="text" class="form-control" id="CreateAblumName" placeholder="AlbumName">
                            </div>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" onclick="createAblum(document.getElementById('CreateAblumName'))">Create</button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Update" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="H1">修改专辑名/删除专辑</h4>
                </div>
                <div class="modal-body">

                    <form class="form-horizontal" role="form">
                        <div class="form-group">
                            <label for="inputEmail3" class="col-sm-3 control-label">专辑名</label>
                            <div class="col-sm-6">
                                <input type="text" class="form-control" id="AblumName" placeholder="AlbumName">
                            </div>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button id="btnDeleteAlbum" type="button" class="btn btn-danger pull-left" onclick="DeleteAblum();">Delete</button>
                    <span id="DeleteAlbumLoading" class=" pull-left" style="display:none;margin:5px;"> <span class="glyphicon glyphicon-refresh tagLoading" ></span> Loading...</span>
                    <button type="button" class="btn btn-primary" onclick="changeAblumName2(this);">Save</button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>


</asp:Content>

