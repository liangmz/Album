﻿<%@ Page Title="" Language="C#" MasterPageFile="~/HD.master" AutoEventWireup="true" CodeFile="ThisPic.aspx.cs" Inherits="ThisPic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/thispic.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BODY" runat="Server">
    <div class="container">
        <div class="thumbnail">
            <img id="pdata" src="" class="img-responsive img-rounded" />
            <div class="caption">
                <ul id="p_tags">
                </ul>
            </div>
        </div>
    </div>
    
    <div class="col-md-4">
        <div class="media">
            <div class="pull-left">
                <img id="pauthor" class="media-object img-rounded" src="" style="width: 64px; height: 64px;" />
            </div>
            <div class="media-body">
                <p id="pname"></p>
                <p id="puser"></p>
                <p id="ptime"></p>
                <p id="pexname"></p>
                <p id="pspace"></p>
                <p id="psize"></p>
                <p id="pdes"></p>

            </div>
        </div>
    </div>

    <div class="col-md-8">
        <form role="form" onsubmit="return reply(replycontent);">
            <div class="form-group">
                <button id="doReply" type="submit" class="btn btn-primary">回复</button><br />
            </div>
            <div class="form-group">
                <textarea id="replycontent" 
                    onfocus="javascript:replycontent.style.height='150px';" 
                    onblur="javascript:replycontent.style.height='100px';" 
                    class="form-control picUpdate" 
                    style="resize:vertical;height:100px;" 
                    required></textarea>
            </div>
            
        </form>
        <div id="UE">
        </div>

        <ul class="pagination" id="page"></ul>
    
    </div>
    

</asp:Content>

