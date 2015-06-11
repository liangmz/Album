﻿
function getPic(pid, title, time, des, src) {
    var div = document.createElement('div');
    div.className = 'thumbnail';

    var picContent = '<a href="ThisPic.aspx?pid=' + pid + '">' +
            '<img src="' + src + '" class="img-responsive img-rounded mypic">' +
        '</a>' +
        '<div onclick="bgChange("pcb' + pid + '");">' +
            '<h6 style="word-break:break-all;">' + title + '</h6>' +
            '<p>' +
                '<a href="javascript:void(0)">' + time + '</a>' +
            '</p>' +
            '<input type="checkBox" pid=' + pid + ' id="pcb' + pid + '" onclick="bgChange(this);">' +
        '</div>';
    div.innerHTML = picContent;
    return div;
}
//overflow:hidden; 内容超过剪去

//图片节点
function getPicNext(pid, title, time, des, src,power) {
    var div = document.createElement('div');
    div.className = 'media';
    div.id = 'ap' + pid;
    //div.style.height = '280px';

    var powerNode = '';
    switch (Number(power)) {
        case 0: { powerNode = '<p style="color:#563d7c;">共享</p>'; } break;
        case 1: { powerNode = '<p style="color:#5cb85c;">关注分享</p>'; } break;
        case 2: { powerNode = '<p style="color:#d2322d;">保密</p>'; } break;
    }

    var picConent = '<div class="thumbnail pull-left" style="width: 40%;">' +
                '<a href="ThisPic.aspx?pid=' + pid + '">' +
                    '<img src="' + src + '" class="media-object" style="width: auto; max-height: 150px;">' +
                '</a>' +
                '<div class="form-group pull-right" style="margin-top: 10px;">' +
                    '<input type="checkBox" class="pull-right"  pid=' + pid + ' id="pcb' + pid + '" onclick="bgChange(this);">' +
                '</div>' +
                '<h5>上传时间: ' + time + '</h5>' +
                '<a href="javascript:void(0);" onclick="getTags(' + pid + ');">Tag</a>' +
                powerNode+
            '</div>' +
            '<div class="media-body">' +
                '<p>图片名：</p>' + 
                '<div class="form-group">' +
                    '<input type="text" class="form-control" id="pn' + pid + '" placeholder="图片名" disabled required value=' + title + ' prevalue>' +
                '</div>' +
                '<p>描述：</p>' +
                '<div class="form-group">' +
                    '<textarea id="pdes' + pid + '" class="form-control picUpdate" placeholder="" style="resize: vertical;height:100px;" prevalue required disabled>' + des + '</textarea>' +
                '</div>' +
                '<div class="form-group">' +
                    '<button type="button" class="btn btn-primary pull-right" onclick="Edit(this,pn' + pid + ',pdes' + pid + ');" pid=' + pid + '>编辑</button>' +
                    '<button type="button" class="btn btn-default" onclick="Cancel(this,pn' + pid + ',pdes' + pid + ');" disabled>撤销</button>' +
                '</div>' +
            '</div>';
    div.innerHTML = picConent;
    return div;
}

//标签节点
function TagNode(tid,tname)
{
    var Node = document.createElement('div');
    Node.className = 'checkbox';
    Node.setAttribute('tid',tid);
    var tag = '<input type="checkbox" value="'+ tid + '">'+
        '<span class="label label-primary">' + tname + '</span>';
    Node.innerHTML = tag;
    return Node;
}

//获取标签列表
function getTags(pid)
{
    sessionStorage.pid = pid;
    $('#Tag').modal('show');
    
    loadTagList(pid);
}
//载入标签列表
function loadTagList(pid)
{
    function AjaxDo(responseText) {
        var xmlDom = getXML(responseText);
        var root = xmlDom.documentElement;
        var TagList = root.getElementsByTagName('TagList')[0];

        var tagList = document.getElementById('tagList');
        tagList.innerHTML = '';

        for (var i = 0 ; i < TagList.childNodes.length ; i++) {
            var tag = TagList.childNodes[i];
            var TID = getXmlNodeValue(tag.getElementsByTagName('TID')[0]);
            var TNAME = getXmlNodeValue(tag.getElementsByTagName('TNAME')[0]);
            tagList.appendChild(TagNode(TID, TNAME));
        }
        document.getElementById('tagLoading').style.display = 'none';
    }
    var data = new FormData();
    data.append('pid', pid);
    DoAjax(data, AjaxDo, "Ashx/TagList.ashx", "POST");
}

//添加标签 标签输入节点
function addTag(tname)
{
    function AjaxDo(responseText)
    {
        var xmlDom = getXML(responseText);
        var root = xmlDom.documentElement;
        var TagAddList = root.getElementsByTagName('TagAddList')[0];

        var tagList = document.getElementById('tagList');

        for (var i = 0 ; i < TagAddList.childNodes.length ; i++) {
            var tag = TagAddList.childNodes[i];
            var TID =  getXmlNodeValue(tag.getElementsByTagName('TID')[0]);
            var TNAME = getXmlNodeValue(tag.getElementsByTagName('TNAME')[0]);
            tagList.appendChild(TagNode(TID, TNAME));
        }
        tname.value = '';
    }

    var data = new FormData();
    data.append('tagName', tname.value);
    data.append('pid', sessionStorage.pid);
    DoAjax(data, AjaxDo, "Ashx/TagAdd.ashx", "POST");
}

//删除标签 存放标签div
function delTag(div)
{
    var tagList = document.getElementById('tagList');
    var cbList = tagList.getElementsByTagName('input');

    var tidList = '';
    for (var i = 0 ; i < cbList.length ; i++)
    {
        if (cbList[i].type != 'checkbox') continue;
        if (!cbList[i].checked) continue;
        tidList += (cbList[i].value + ',');
    }

    function AjaxDo(responseText)
    {
        var reTidList = responseText.split(',');
        if (reTidList.length > 0)
        {
            //重新载入标签
            document.getElementById('tagLoading').style.display = 'block';
            loadTagList(sessionStorage.pid);
        }
    }

    var data = new FormData();
    data.append('pid', sessionStorage.pid);
    data.append('tid', tidList);

    DoAjax(data, AjaxDo, "Ashx/TagDel.ashx", "POST");
}

//xmlhttprequest发送
function DoAjax(data, fun, url, mode) {
    var xhr = getXmlHttpRequest();
    xhr.open(mode, url, true);
    xhr.send(data);
    xhr.onreadystatechange = xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            fun(xhr.responseText);
        }
    }
}


//权限更新
function PowerUpdate(e)
{
    var pidList = checkList();
    //alert(pidList);
    //return false;
    var aid = '';

    var data = new FormData();
    data.append('pidList', pidList);
    data.append('aid', window.aid);
    data.append('power',e.getAttribute('value'))

    var xhr = getXmlHttpRequest();
    xhr.open('POST', 'Ashx/UpdatePicPower.ashx', true);
    xhr.send(data);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            if (xhr.responseText != '')
                {
                pidList = xhr.responseText;
                var List = new Array();
                List = pidList.split(',');
                for (var i = 0 ; i < List.length ; i++) {
                    if (List[i] == undefined || List[i] == '') continue;
                    var id = 'ap' + List[i];
                   // document.getElementById('id').remove();
                }
                //alert(List);
                var colorValue = document.getElementById('PowerUpload').getAttribute('value');
                var color = '';
                switch (Number(colorValue)) {
                    case 0: { color = '#563d7c'; } break;
                    case 1: { color = '#5cb85c'; } break;
                    case 2: { color = '#d2322d'; } break;
                }
                var powerText = '';
                switch (Number(colorValue)) {
                    case 0: { powerText = '共享'; } break;
                    case 1: { powerText = '关注分享'; } break;
                    case 2: { powerText = '保密'; } break;
                }
                for (var i = 0; i < List.length ; i++)
                {
                    var Node = document.getElementById('ap' + List[i]);
                    if (Node == undefined) continue;
                    Node.children[0].children[4].style.color = color;
                    Node.children[0].children[4].innerHTML = powerText;
                }
                alert('Completed');
            }
        }
    }
}

//修改专辑
function AlbumUpdate(aidSelected)
{
    if (window.aid == aidSelected.value)
        return false;

    var pidList = checkList();

    var data = new FormData();
    data.append('pidList', pidList);
    data.append('aid', aidSelected.value);

    var xhr = getXmlHttpRequest();
    xhr.open('POST', 'Ashx/UpdatePicAlbum.ashx', true);
    xhr.send(data);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200)
        {
            if (xhr.responseText != '')
            {
                pidList = xhr.responseText;
                var List = new Array();
                List = pidList.split(',');
                for (var i = 0 ; i < List.length ; i++)
                {
                    if (List[i] == undefined || List[i] == '') continue;
                    var id = 'ap' + List[i];
                    document.getElementById(id).parentNode.removeChild(document.getElementById(id));//.remove();
                }
            }
        }
    }
}

//复选框选中
function checkList()
{
    var pidList = '';
    //alert('删除未实现');
    var cbList = document.getElementsByTagName('input');
    for (var i = 0 ; i < cbList.length ; i++) {
        if (cbList[i].type != 'checkbox') continue;
        if (cbList[i].getAttribute('pid') == undefined) continue;
        if (!cbList[i].checked) continue;
        var pid = cbList[i].getAttribute('pid');
        pidList += pid + ',';
    }
    pidList = pidList.substring(0, pidList.length - 1);
    return pidList;
}

//删除
function SelectDeletePic()
{
    var pidList = checkList();;

    var data = new FormData();
    data.append('pidList', pidList);

    var xhr = getXmlHttpRequest();
    xhr.open("POST", "Ashx/PicRemove.ashx", true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            if (xhr.responseText == '-1' || xhr.responseText == '0') {
                alert('更新失败');
            }
            else {
                alert('更新成功');
                changePageActive(sessionStorage.page);
            }
        }
    }
    xhr.send(data);

}
//撤销
function Cancel(e, pn, pdes) {
    pn.value = pn.prevalue;
    pdes.value = pdes.prevalue;

    var F_save = function () {
        var save = e.previousSibling;
        while (save.nodeType != 1) {
            save = save.previousSibling;
        }
        return save;
    }

    var save = F_save();
    e.setAttribute('disabled', 'true');
    pn.setAttribute('disabled', 'true');
    pdes.setAttribute('disabled', 'true');
    save.innerHTML = "编辑";
    pdes.style.height = '100px';
    save.setAttribute('onclick', 'Edit(this,' + pn.id + ',' + pdes.id + ');');

}
//点击编辑按钮
function Edit(e,pn,pdes)
{
    pn.prevalue = pn.value;
    pdes.prevalue = pdes.value;

    var F_cancel = function () {
        var edit = e.nextSibling;
        while (edit.nodeType != 1) {
            edit = edit.nextSibling;
        }
        return edit;
    };
    var cancel = F_cancel();
    cancel.disabled = false;
    pn.disabled = false;
    pdes.disabled = false;
    e.innerHTML = "保存";
    pdes.style.height = '150px';
    e.setAttribute('onclick', 'Save(this,' + pn.id + ',' + pdes.id + ');');
}
//点击保存按钮
function Save(e, pn, pdes)
{
    var funSave = function () {
        pn.prevalue = '';
        pdes.prevalue = '';
        var F_cancel = function () {
            var save = e.nextSibling;
            while (save.nodeType != 1) {
                save = save.nextSibling;
            }
            return save;
        };
        var cancel = F_cancel();
        cancel.setAttribute('disabled', 'true');
        pn.setAttribute('disabled', 'true');
        pdes.setAttribute('disabled', 'true');
        e.innerHTML = "编辑";
        pdes.style.height = '100px';
        e.setAttribute('onclick', 'Edit(this,' + pn.id + ',' + pdes.id + ');');
    }

    if (pn.value == pn.prevalue && pdes.value == pdes.prevalue) {
        funSave();
        return;
    }
    if (pn.value == '' || pdes.value == '') {
        alert('不可以为空');
        return;
    }

    var data = new FormData();
    //data.append('aid', window.aid);
    data.append('pid', e.getAttribute('pid'));
    data.append('picname', pn.value);
    data.append('content', pdes.value);

    var xhr = getXmlHttpRequest();
    xhr.open("POST", "Ashx/UpdatePic.ashx", true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            if (xhr.responseText == '-1' || xhr.responseText == '0') {
                pn.value = pn.prevalue;
                pdes.value = pdes.prevalue;
                alert('更新失败');
            }
            else {
                alert('更新成功');
            }
            funSave();
        }
    }
    xhr.send(data);

    
}


//checkBox选中状态修改
function bgChange(e)
{
    var par = e.parentNode.parentNode;
    if (e.checked) {
        e.checked = true;
        par.style.backgroundColor = '#f0f0f0';
    }
    else
    {
        e.checked = false;
        par.style.backgroundColor = '#ffffff';
    }
}

//取消ocheckBox
function SelectCancelCheckBox()
{
    var cbList = document.getElementsByTagName('input');
    for (var i = 0 ; i < cbList.length ; i++) {
        console.log(cbList[i]);
        if (cbList[i].type != 'checkbox') continue;
        if (cbList[i].getAttribute('pid') == undefined) continue;
        checkBoxUnActive(cbList[i]);
    }
}

//全选checkBox
function SelectAllCheckBox()
{
    var cbList = document.getElementsByTagName('input');
    for (var i = 0 ; i < cbList.length ; i++)
    {
        if (cbList[i].type != 'checkbox') continue;
        if (cbList[i].getAttribute('pid') == undefined) continue;
        checkBoxActive(cbList[i]);
    }
}
//反选checkBox
function SelectUnCheckBox() {
    var cbList = document.getElementsByTagName('input');
    for (var i = 0 ; i < cbList.length ; i++) {
        console.log(cbList[i]);
        if (cbList[i].type != 'checkbox') continue;
        if (cbList[i].getAttribute('pid') == undefined) continue;
        bgChange(cbList[i]);
        cbList[i].checked ? checkBoxUnActive(cbList[i]) : checkBoxActive(cbList[i]);
    }
}
//选择checkBox
function checkBoxActive(e)
{
    e.checked = true;
    var par = e.parentNode.parentNode;
    par.style.backgroundColor = '#f0f0f0';
}
//取消checkBox
function checkBoxUnActive(e) {
    e.checked = false;
    var par = e.parentNode.parentNode;
    par.style.backgroundColor = '#ffffff';
}