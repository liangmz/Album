<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default_Login.aspx.cs" Inherits="Default_Login" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Album</title>
    <script src="dist/js/jquery.js"></script>
    <link rel="stylesheet" href="dist/css/bootstrap.min.css" media="screen" />

    <script src="dist/js/bootstrap.min.js"></script>
    <link href="css/mycss.css" rel="stylesheet" />

    <link rel="Shortcut Icon" href="Image/Logo.ico" />
    <!--地址栏和标签上显示图标-->
    <link rel="Bookmark" href="Image/Logo.ico" />
    <!--收藏夹显示图标-->
</head>
<body style="background-color: #563d7c">
    <script type="text/javascript">
        function GoUserName() {
            D_Password.style.right = '-100%';
            D_Username.style.right = '5%';
        }
        function GoPassword() {
            D_Password.style.right = '5%';
            D_Username.style.right = '-100%';
            Password.focus();
        }

        /*获得异步请求*/
        function getXmlHttpRequest() {
            var request;
            if (window.XMLHttpRequest)
                request = new XMLHttpRequest();
            else
                try {
                    request = new ActiveXObject("Microsoft.XMLHTTP");
                } catch (e) {
                    request = new ActiveXObject("Msxml2.XMLHTTP");
                };
            return request;
        }
        function Login() {
            var UserName = document.getElementById('UserName');
            var Password = document.getElementById("Password");
            var data = new FormData();
            data.append("userName", UserName.value);
            data.append("password", Password.value);

            var xmlhttprequest = getXmlHttpRequest();
            xmlhttprequest.open("post", "Ashx/Login.ashx", true);
            xmlhttprequest.onreadystatechange = function () {
                if (xmlhttprequest.readyState == 4 && xmlhttprequest.status == 200) {
                    if (xmlhttprequest.responseText != '-1') {
                        location.replace('./Default.aspx');
                    }
                    else {
                        GoUserName();
                    }
                    return false;
                }
            }
            xmlhttprequest.send(data);
            return false;
        }
    </script>
    <style type="text/css">
        .psw {
            transform: rotateY(180deg);
            -webkit-transform: rotateY(180deg); /* Safari and Chrome */
        }
    </style>

    <div class="container" style="padding-top:6%;">
        <h1 style="color:#fff" class="pull-right">ALBUM</h1>
    </div>
    <form onsubmit="return Login();">
        <div id="D_Username" class="input-group col-lg-2 col-xs-6 login" style="position: fixed; right: 5%; top: 60%;">
            <input id="UserName" name="UserName" type="text" tabindex="0" onblur="GoPassword();" class="form-control" placeholder="UserName">
            <span class="input-group-btn">
                <button class="btn btn-default" type="button" onclick="GoPassword();"><span class="glyphicon glyphicon-share-alt"></span></button>
            </span>
        </div>
        <div id="D_Password" class="input-group col-lg-3 col-xs-7 login" style="position: fixed; right: -100%; top: 60%;">
            <span class="input-group-btn">
                <button class="btn btn-default" type="button" onclick="GoUserName();"><span class="glyphicon glyphicon-share-alt psw"></span></button>
            </span>
            <input id="Password" name="Password" type="password" tabindex="1" class="form-control picUpdate" placeholder="Password">
            <span class="input-group-btn">
                <button class="btn btn-default" type="submit">SIGN IN</button>
            </span>

        </div>
    </form>

</body>
</html>
