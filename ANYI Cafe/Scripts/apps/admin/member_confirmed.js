﻿$(document).ready(function () {
    var oTable = $('#DatatableList').DataTable({
        "ajax": {
            "url": '/Admin/Member/GetDataList',
            "type": "get",
            "datatype": "json"
        },
        "language": {
            "sProcessing": "處理中...",
            "sLengthMenu": "顯示 _MENU_ 項結果",
            "sZeroRecords": "沒有匹配結果",
            "sInfo": "顯示第 _START_ 至 _END_ 項結果，共 _TOTAL_ 項",
            "sInfoEmpty": "顯示第 0 至 0 項結果，共 0 項",
            "sInfoFiltered": "(從 _MAX_ 項結果過濾)",
            "sInfoPostFix": "",
            "sSearch": "查詢:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "首頁",
                "sPrevious": "上頁",
                "sNext": "下頁",
                "sLast": "尾頁"
            }
        },
        "columns": [
            {
                "data": "rowid", "width": "60px", "orderable": false, "render": function (data) {
                    return '<a title="取消登入" href="/Admin/Member/UnConfirm/' + data + '" onclick="return confirm(\'是否確定要取消登入權限 ?\')"><i class="fas fa-unlock fa-2x"></i></a>';
                }
            },
            {
                "data": "isvarify", "width": "30px", "title": "驗證", "render": function (data) {
                    if (data == 1) { return '<input type="checkbox" checked="checked" disabled="disabled" />'; } else { return '<input type="checkbox" disabled="disabled" />'; }
                }
            },
            { "data": "mno", "width": "30px", "title": "編號" },
            { "data": "mname", "width": "60px", "title": "姓名" },
            {
                "data": "birthday", "width": "70px", "title": "出生日期",
                "render": function (data) { return moment(data).format("YYYY/MM/DD"); }
            },
            { "data": "email", "width": "200px", "title": "電子郵件" },
            { "data": "remark", "autoWidth": true, "title": "備註" }
        ],
    })
    $('.tablecontainer').on('click', 'a.popup', function (e) {
        e.preventDefault();
        OpenPopup($(this).attr('href'));
    })
    function OpenPopup(pageUrl) {
        var $pageContent = $('<div />');
        $pageContent.load(pageUrl, function () {
            $('#popupForm', $pageContent).removeData('validator');
            $('#popupForm', $pageContent).removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse('form');

        });

        $dialog = $('<div class="popupWindow" style="overflow:auto"></div>')
            .html($pageContent)
            .dialog({
                draggable: true,
                autoOpen: false,
                resizable: true,
                model: true,
                title: '編輯視窗',
                height: 540,
                width: 400,
                close: function () {
                    $dialog.dialog('destroy').remove();
                }
            })

        $('.popupWindow').on('submit', '#popupForm', function (e) {
            var url = $('#popupForm')[0].action;
            $.ajax({
                type: "POST",
                url: url,
                data: $('#popupForm').serialize(),
                success: function (data) {
                    if (data.status) {
                        $dialog.dialog('close');
                        oTable.ajax.reload();
                    }
                }
            })
            e.preventDefault();
        })
        $dialog.dialog('open');
    }
})