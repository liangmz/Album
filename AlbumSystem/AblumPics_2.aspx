﻿<%@ Page Title="" Language="C#" MasterPageFile="~/HD.master" AutoEventWireup="true" CodeFile="AblumPics_2.aspx.cs" Inherits="AblumPics_2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/myPic.js"></script>
    <script src="js/ablumpic.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BODY" runat="Server">
    <div class="alert alert-info">
        <button type="button" id="UP" class="btn btn-primary" data-toggle="modal" data-target="#Upload" style="margin:5px;">上传图片</button>
        <button type="button" id="SelectAll" class="btn btn-primary" onclick="SelectAllCheckBox();" style="margin:5px;">全部选中</button>
        <button type="button" id="SelectUn" class="btn btn-primary" onclick="SelectUnCheckBox();" style="margin:5px;">全部反选</button>
        <button type="button" id="SelectCancel" class="btn btn-primary" onclick="SelectCancelCheckBox();" style="margin:5px;">全部取消</button>
        |
        <button type="button" id="Delete" class="btn btn-primary" onclick="SelectDeletePic();" style="margin:5px;">选中删除</button>
        |
        <div class="btn-group">
            <button id="powers" type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" style="margin:5px;">
                权限 <span class="caret"></span>
            </button>
            <ul class="dropdown-menu" role="menu">
                <li><a href="javascript:void(0);" value="0" onclick="buttonDownMenuAction(this,document.getElementById('PowerUpload'),document.getElementById('powers'));">共享</a></li>
                <li><a href="javascript:void(0);" value="1" onclick="buttonDownMenuAction(this,document.getElementById('PowerUpload'),document.getElementById('powers'));">关注分享</a></li>
                <li><a href="javascript:void(0);" value="2" onclick="buttonDownMenuAction(this,document.getElementById('PowerUpload'),document.getElementById('powers'));">保密</a></li>
            </ul>
        </div>
        <button type="button" id="PowerUpload" class="btn btn-primary" onclick="PowerUpdate(this);" value="0" style="margin:5px;">选中权限更新</button>
        |
        <div class="btn-group" style="margin:5px;">
            <%
                localhost.AblumService AS = new localhost.AblumService();
                AS.CookieContainer = (CookieContainer)Session["cookie"];
                string Album = AS.MyAlbumList();
                XmlDocument XML = new XmlDocument();
                XML.LoadXml(Album);
                XmlElement AblumData = XML.DocumentElement;
                XmlElement MyAlbumList = AblumData["MyAlbumList"];
            %>
            <select class="form-control" id="aidSelected">
                <%for (int i = 0; i < (MyAlbumList == null ? 0 : MyAlbumList.ChildNodes.Count); i++)
                  { %>
                <option value="<%=MyAlbumList.ChildNodes[i]["AID"].InnerText %>"><%=MyAlbumList.ChildNodes[i]["ANAME"].InnerText %> </option>
                <%} %>
            </select>
        </div>
        <button type="button" id="AlbumUpdate" class="btn btn-primary" onclick="AlbumUpdate(document.getElementById('aidSelected'));" value="0" style="margin:5px;">选中专辑更新</button>
        
    </div>
    <ol class="breadcrumb">
        <li><a href="MyAblum.aspx">Ablum</a></li>
        <li class="active" id="AblumName"></li>
    </ol>

    <div class="col-lg-6" id="PA">
    </div>

    <div class="col-lg-6" id="PB">
    </div>

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
                                <input id="p" type="file" required multiple="multiple" accept="image/*" onchange="up_File_List_Do(this,document.getElementById('up_File_List'));" />
                            </div>
                        </div>
                    </form>
                    <div class="btn-group">
                        <select class="form-control" id="UploadPower">
                        <option selected value="0">共享</option>
                        <option value="1">关注分享</option>
                        <option value="2">保密</option>
                    </select>
                    </div>
                    <div id="up_File_List" class="form-group" style="margin-top:10px;margin-left:1px;">
                        
                    </div>
                </div>
                <div class="modal-footer">
                    <span id="UploadLoading" class="pull-left" style="display:none;margin:5px;"> <span class="glyphicon glyphicon-refresh tagLoading" ></span> Loading...</span>
                    <button id="uploadBtn" type="button" class="btn btn-primary" onclick="uploadPic(document.getElementById('p'),document.getElementById('UploadPower'),this)">Upload</button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>

            </div>
        </div>
    </div>

    <div class="modal fade" id="Tag" tabindex="-1" role="dialog" aria-labelledby="tagShow" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title" id="tagShow">标签</h4>
                    </div>

                    <div class="modal-body form-horizontal" id="TagBody">
                        <form role="form" onsubmit="return Login();">
                            <div>
                                <div class="input-group col-sm-4">
                                    <input id="NewTag" type="text" class="form-control" placeholder="Tag" required />
                                    <span class="input-group-btn">
                                        <button class="btn btn-primary" onclick="addTag(NewTag);">Add</button>
                                    </span>
                                </div>
                            </div>
                        </form>
                        <div id="tagLoading" class="picUpdate" style="padding-top:10px;padding-bottom:10px;">
                            <span class="glyphicon glyphicon-refresh tagLoading"></span>
                            载入中...
                        </div>
                        <div id="tagList" class="picUpdate"  style="padding-top:10px;padding-bottom:10px;">
                        </div>
                    </div>
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger pull-left" onclick="delTag(document.getElementById('tagList'));">DeleteTag</button>
                        <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                    </div>

                </div>
            </div>
        </div>
</asp:Content>

