
window.onload = function () {
    window.fpid = 1;
    window.aid = request('aid');
    document.getElementById('AblumName').innerHTML = request('AN');
    changePageActive(1);
    //buildPage(sessionStorage.pageCount, 1, 'return changePage(this)');

    $('#Tag').on('hidden.bs.modal', function (e) {
        document.getElementById('tagList').innerHTML = '';
        document.getElementById('tagLoading').style.display = 'block';
    });
}

/*读取URL中get参数,网上解决方案*/
function request(strParame) {
    var args = new Object();
    var query = location.search.substring(1);

    var pairs = query.split("&"); // Break at ampersand 
    for (var i = 0; i < pairs.length; i++) {
        var pos = pairs[i].indexOf('=');
        if (pos == -1) continue;
        var argname = pairs[i].substring(0, pos);
        var value = pairs[i].substring(pos + 1);
        value = decodeURIComponent(value);/*decodeURIComponent() 函数可对 encodeURIComponent() 函数编码的 URI 进行解码。*/
        args[argname] = value;
    }
    return args[strParame];
}

function getE(type) {
    return document.createElement(type);
}

function uploadPic(e, UploadPower) {
    //var uploadPictures = document.getElementById('uploadPictures');
    document.getElementById('uploadBtn').setAttribute('disabled', 'disabled');
    document.getElementById('UploadLoading').style.display = 'block';
    var pics = new FormData();

    var fp = document.getElementById('p');
    for (var i = 0 ; i < fp.files.length ; i++) {
        pics.append('f' + i, fp.files[i]);
    }
    pics.append('aid', aid);
    pics.append('power', UploadPower.value);

    var xmlrequest = getXmlHttpRequest();
    xmlrequest.open('post', 'Ashx/Upload.ashx', true);
    xmlrequest.onreadystatechange = function () {
        if (xmlrequest.readyState == 4 && xmlrequest.status == 200) {
            if (xmlrequest.responseText == '-1') {
                $('#Lose').modal('show');
                $('#Upload').modal('hide');
            }
            else {
                $('#Success').modal('show');
                $('#Upload').modal('hide');
                //changePageActive(page);
                changePageActive(sessionStorage.page);
            }
            document.getElementById('UploadLoading').style.display = 'none';
            try { document.getElementById('uploadBtn').removeAttribute('disabled'); } catch (e) { };
            e.value = '';
            document.getElementById('up_File_List').innerHTML = '';
        }
    }
    xmlrequest.send(pics);
}

function changePageActive(_page) {
    sessionStorage.page = _page;

    var data = new FormData();
    data.append('page', _page);
    data.append('aid', aid);

    var xmlrequest = getXmlHttpRequest();
    xmlrequest.open("post", "Ashx/AblumPics.ashx", true);
    xmlrequest.onreadystatechange = function () {
        if (xmlrequest.readyState == 4 && xmlrequest.status == 200) {
            PA.innerHTML = '';
            PB.innerHTML = '';
            var xml = xmlrequest.responseText;
            //alert(xml);
            var xmlDom = getXML(xml);
            var root = xmlDom.documentElement;
            sessionStorage.pageCount = getXmlNodeValue(root.getElementsByTagName('AblumPicCount')[0]);
            var Share = root.getElementsByTagName('AblumPicList')[0];

            var E = function () {
                var Min = PA.offsetHeight <= PB.offsetHeight ? PA : PB;
                return Min;

            };
            //建立结果页面
            for (var i = 0 ; i < Share.childNodes.length ; i++) {
                var user = Share.childNodes[i];
                var PID = getXmlNodeValue(user.getElementsByTagName('PID')[0]);
                var PNAME = getXmlNodeValue(user.getElementsByTagName('PNAME')[0]);
                var PTIME = getXmlNodeValue(user.getElementsByTagName('PTIME')[0]);
                var PDES = getXmlNodeValue(user.getElementsByTagName('PDES')[0]);
                var PDATA = getXmlNodeValue(user.getElementsByTagName('PDATA')[0]);
                var POWER = getXmlNodeValue(user.getElementsByTagName('PPOWER')[0]);
                var MIN = E();
                MIN.appendChild(getPicNext(PID, PNAME, PTIME , PDES, PDATA,POWER));
            }
            buildPage(sessionStorage.pageCount, sessionStorage.page, 'return changePage(this)');
        }
    }
    xmlrequest.send(data);
}

//上传文件确定后打印文件名列表
function up_File_List_Do(e, up_File_List) {
    up_File_List.innerHTML = '';
    for (var i = 0 ; i < e.files.length ; i++) {
        up_File_List.innerHTML += '<p>' + e.files[i].name + '</p>';
    }
}