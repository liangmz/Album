﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default_Login_.aspx.cs" Inherits="Default_Login_" %>

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
<body>
    <script type="text/javascript">

        window.onload = function ()
        {
            document.getElementById('UserName').value = window.localStorage['UserName'] ? window.localStorage['UserName'] : '';
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
            var remenberMe = document.getElementById('remenberMe');

            if (window.localStorage && remenberMe.checked) {
                window.localStorage['UserName'] = UserName.value;
            } else if (window.localStorage && !remenberMe.checked) {
                window.localStorage.removeItem('UserName');
            }

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
        body {
            padding-top: 40px;
            padding-bottom: 40px;
            background-color: #eee;
        }

        .form-signin {
            max-width: 330px;
            padding: 15px;
            margin: 0 auto;
        }

            .form-signin .form-signin-heading,
            .form-signin .checkbox {
                margin-bottom: 10px;
            }

            .form-signin .checkbox {
                font-weight: normal;
            }

            .form-signin .form-control {
                position: relative;
                font-size: 16px;
                height: auto;
                padding: 10px;
                -webkit-box-sizing: border-box;
                -moz-box-sizing: border-box;
                box-sizing: border-box;
            }

                .form-signin .form-control:focus {
                    z-index: 2;
                }

            .form-signin input[type="text"] {
                margin-bottom: -1px;
                border-bottom-left-radius: 0;
                border-bottom-right-radius: 0;
            }

            .form-signin input[type="password"] {
                margin-bottom: 10px;
                border-top-left-radius: 0;
                border-top-right-radius: 0;
            }
    </style>
    <div class="container">

        <form class="form-signin" role="form" onsubmit="return Login();">
            <center><h2 class="form-signin-heading"><a href="Default.aspx">ALBUM</a></h2></center>
            <input id="UserName" name="UserName" tabindex="1" type="text" class="form-control login" placeholder="UserName" required autofocus>
            <input id="Password" name="Password" tabindex="2" type="password" class="form-control login" placeholder="Password" required>
            <label class="checkbox">
                <a class="pull-right login" tabindex="-1" href="ForgetPassword.aspx">忘记密码?</a>
                <input id="remenberMe" tabindex="3" type="checkbox" value="remember-me">
                Remember me
            </label>
            <button class="btn btn-lg btn-primary btn-block login" tabindex="4" type="submit">Sign in</button>
            <button class="btn btn-lg btn-primary btn-block login" tabindex="4" type="button" onclick="javascript:location.href='./Default.aspx'">进入系统</button>
        </form>
        
    </div>
</body>
</html>
