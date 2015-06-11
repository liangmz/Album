
window.onload = function ()
{
    if (document.getElementById('userName').innerHTML == '')
    {
        replyDisable();
    }
    else { replyEnable(); }
    getPic(request('pid'));
    getPicTags(request('pid'));
    changePageActive(1);
    //buildPage(sessionStorage.pageCount, 1, 'return changePage(this)');
}

//不可编辑回复
function replyDisable()
{
    document.getElementById('doReply').setAttribute('disabled', 'disabled');
}
//可编辑回复
function replyEnable()
{
    document.getElementById('doReply').removeAttribute('disabled');
}

//回复换页
function changePageActive(_page)
{
    var pid = request('pid');
    getRely(_page, pid);
}

/*读取URL中get参数*/
function request(strParame) {
    var args = new Object();
    var query = location.search.substring(1);

    var pairs = query.split("&"); // Break at ampersand 
    for (var i = 0; i < pairs.length; i++) {
        var pos = pairs[i].indexOf('=');
        if (pos == -1) continue;
        var argname = pairs[i].substring(0, pos);
        var value = pairs[i].substring(pos + 1);
        value = decodeURIComponent(value);
        args[argname] = value;
    }
    return args[strParame];
}

//请求图片标签数据
function getPicTags(pid) {

    function AjaxDo(responseText) {
        var xmlDom = getXML(responseText);
        var root = xmlDom.documentElement;
        var TagList = root.getElementsByTagName('TagList')[0];

        var tagList = document.getElementById('p_tags');
        tagList.innerHTML = '';

        for (var i = 0 ; i < TagList.childNodes.length ; i++) {
            var tag = TagList.childNodes[i];
            var TNAME = getXmlNodeValue(tag.getElementsByTagName('TNAME')[0]);
            tagList.innerHTML += getSpan(TNAME);
        }
    }
    var data = new FormData();
    data.append('pid', pid);
    DoAjax(data, AjaxDo, "Ashx/TagList.ashx", "POST");
}


//请求图片数据
function getPic(pid)
{
    var data = new FormData();
    data.append('pid',pid);

    var xmlrequest = getXmlHttpRequest();
    xmlrequest.open('post', 'Ashx/ThePic.ashx', true);
    xmlrequest.onreadystatechange = function () {
        if (xmlrequest.readyState == 4 && xmlrequest.status == 200) {
            var xml = xmlrequest.responseText;
            var xmlDom = getXML(xml);
            var root = xmlDom.documentElement;
            if (getXmlNodeValue(root) == '')
            {
                window.location.href = './Error.aspx';
                return;
            }
            var ThePicture = root.getElementsByTagName('ThePicture')[0];

            var PID = getXmlNodeValue(ThePicture.getElementsByTagName('PID')[0]);
            var PUSERNAME = getXmlNodeValue(ThePicture.getElementsByTagName('USERNAME')[0]);
            var PNAME = getXmlNodeValue(ThePicture.getElementsByTagName('PNAME')[0]);
            var PTIME = getXmlNodeValue(ThePicture.getElementsByTagName('PTIME')[0]);
            var PDES = getXmlNodeValue(ThePicture.getElementsByTagName('PDES')[0]);
            var PDATA = getXmlNodeValue(ThePicture.getElementsByTagName('PDATA')[0]);
            var AUTHORPIC = getXmlNodeValue(ThePicture.getElementsByTagName('AUTHORPIC')[0]);
            var PHEIGHT = getXmlNodeValue(ThePicture.getElementsByTagName('PHEIGHT')[0]);
            var PWIDTH = getXmlNodeValue(ThePicture.getElementsByTagName('PWIDTH')[0]);
            var PSPACE = getXmlNodeValue(ThePicture.getElementsByTagName('PSPACE')[0]);
            var PEXNAME = getXmlNodeValue(ThePicture.getElementsByTagName('PEXNAME')[0]);

            var img = document.getElementById('pdata');
            img.src = PDATA;

            /*添加图片用户信息*/
            document.getElementById('pauthor').src = AUTHORPIC;
            document.getElementById('pname').innerHTML = '图片名：' + PNAME;
            document.getElementById('puser').innerHTML = '上传者：' + PUSERNAME;
            document.getElementById('ptime').innerHTML = '上传时间：' + PTIME;
            document.getElementById('pexname').innerHTML = '格式：' + PEXNAME;
            document.getElementById('pspace').innerHTML = '尺寸：' + PHEIGHT + '×' + PWIDTH;
            document.getElementById('psize').innerHTML = '大小：' + PSPACE + 'KB';
            document.getElementById('pdes').innerHTML = '描述：'+PDES;

            delete xmlrequest;
            xmlrequest = null;
        }
    }
    xmlrequest.send(data);
}

//请求回复数据
function getRely(_page,pid)
{
    sessionStorage.page = _page;

    var data = new FormData();
    data.append('page', _page);
    data.append('pid', pid);

    var xhr = getXmlHttpRequest();
    xhr.open('post', 'Ashx/getReply.ashx', true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            document.getElementById('UE').innerHTML = '';
            var xml = xhr.responseText;
            var xmlDom = getXML(xml);
            var root = xmlDom.documentElement;
            var Evaluations = root.getElementsByTagName('Evaluations')[0];
            sessionStorage.pageCount = getXmlNodeValue(root.getElementsByTagName('EvaluationCount')[0]);

            
            for (var i = 0 ; i < Evaluations.childNodes.length; i++)
            {
                var child = Evaluations.childNodes[i];
                var uid = getXmlNodeValue(child.getElementsByTagName('UID')[0]);
                var username = getXmlNodeValue(child.getElementsByTagName('USERNAME')[0]);
                var time = getXmlNodeValue(child.getElementsByTagName('ETIME')[0]);
                var content = getXmlNodeValue(child.getElementsByTagName('EVALUATION')[0]);
                var pic = getXmlNodeValue(child.getElementsByTagName('PIC')[0]);

                document.getElementById('UE').appendChild(getReplyPoint(username,time,content,pic));
            }

            buildPage(sessionStorage.pageCount, sessionStorage.page, 'return changePage(this)');

            delete xhr;
            xhr = null;
        }
    }
    xhr.send(data);
}

//回复
function reply(e)
{
    var data = new FormData();
    data.append('content', e.value);
    data.append('pid', request('pid'));

    var xhr = getXmlHttpRequest();
    xhr.open('post', 'Ashx/Reply.ashx', true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            if (xhr.responseText == '1') {
                $('#Success').modal('show');
                changePageActive(sessionStorage.page);
                e.value = '';
            }
            else {
                $('#Lose').modal('show');
            }
            delete xhr;
            xhr = null;
        }
    }
    xhr.send(data);
    return false;
}


/*回复列表项*/
function getReplyPoint(userName, time, content,data)//data头像
{
    var div = document.createElement('div');
    div.className = 'media';
    var div_img = document.createElement('div');
    div_img.className = 'pull-left';
    var img = document.createElement('img');
    img.className = 'media-object img-rounded';
    img.src = data;
    img.style.width = '64px';
    img.style.height = '64px';
    div_img.appendChild(img);
    div.appendChild(div_img);

    var div2 = document.createElement('div');
    div2.className = 'media-body';
    var h4 = getE('h4');
    h4.className = 'media-heading';
    var a = getE('a');
    a.href = 'javascript:void(0);';
    a.innerHTML = userName;

    h4.appendChild(a);
    div2.appendChild(h4);
    
    var p2 = getE('p');
    p2.innerHTML = content;
    div2.appendChild(p2);
    div.appendChild(div2);

    var p = getE('p');
    p.innerHTML = time;
    div2.appendChild(p);

    return div;
}

function getE(type)
{
    return document.createElement(type);
}

function getSpan(tname)
{
    return '<li><span class="label label-primary">' + tname + '</span></li>';
}