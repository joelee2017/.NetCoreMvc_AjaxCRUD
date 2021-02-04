/***************** 功能 - 列上新增輸入框 *****************/
// html 範例
// 自訂名稱，請名稱一致!!
//<div class="col-md-8">//   
//    <div class="input-group">
//        <input type="text" class="form-control" data-add="button" data-set="自訂名稱">
//            <div class="input-group-append">
//                <button type="button" class="btn btn-primary" data-add="button" data-button="自訂名稱"><i class="fa fa-plus" aria-hidden="true"></i></button>
//            </div>
//		</div>
//      <span class="text-danger d-none" data-button="自訂名稱">不可為空白及空格。</span>
//       <div class="pt-2"  data-button="自訂名稱" style="overflow-y:auto;max-height:250px">
//         寫入 input 區域
//       </div>
//</div>
let addInputButton = (function () {
    let that = {};
    that.init = function () {
        // 按鍵_新增輸入框
        $('button[data-add="button"]').off('click').on('click', function () {
            let target = this.attributes["data-button"].value;
            let setValue = $('input[data-set="' + target + '"]').val();
            // 驗證
            if (setValue == null || setValue.replace(/ /g, "").length == 0) {
                $('span[data-button="' + target + '"]').removeClass('d-none');
                return false;
            }

            let now = new Date().getTime();
            let html = '<div class="input-group ' + now + '">' +
                '<input type="text" class="form-control mb-2" name="' + target + '" data-button="input" value="' + setValue + '" disabled="disabled"/>' +
                '<div class="input-group-append">' +
                '<button type="button" class="btn btn-danger mb-2" data-button="remove" value="' + now + '" ><i class="fa fa-times" aria-hidden="true"></i></button>' +
                '</div>' +
                '</div>';

            $('div[data-button="' + target + '"]').append(html);
            $('input[data-set="' + target + '"]').val('');
        });

        // 值變化(不可 空格及 無值)
        $('input[data-add="button"]').off('keyup').on('keyup', function () {
            let target = this.attributes["data-set"].value;
            if (target != null && target.replace(/ /gi, "").length != 0) {
                $('span[data-button="' + target + '"]').addClass('d-none');
            }
        });

        // 刪除該列 - 繫結事件;
        $('div').delegate('button[data-button="remove"]', 'click', function () {
            $('div.' + this.value + '').remove();
        });
    }

    // 編輯時
    that.readInput = function () {
        return function (setValue, targetName, index) {
            let targetNow = new Date().getTime() + index;
            let html = '<div class="input-group  ' + targetNow + '">' +
                '<input type="text" class="form-control mb-2" name="' + targetName + '" data-button="input" value="' + setValue + '" disabled="disabled"/>' +
                '<div class="input-group-append" data-button="remove">' +
                '<button type="button" class="btn btn-danger mb-2" data-button="remove" value="' + targetNow + '" ><i class="fa fa-times" aria-hidden="true"></i></button>'
            '</div>' +
                '</div>';
            return html;
        };
    }

    // 回傳物件
    return that;

}());

/***************** 功能 - formData 轉 object 原生鏈註冊 *****************/
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
}

/***************** 功能 - formData 轉 object 直接叫用 *****************/
function SerializeObject(selector) {
    var o = {};
    var a = selector.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};


/***************** 功能 - 關閉BootstrapModal *****************/
function ClosedModal() {
    $('button[data-dismiss="modal"]').trigger('click');
    $('.modelchild').modal('hide');
}

/***************** 功能 - 新增&&編輯 Submit *****************/
let activityEvent = (function () {
    let that = {};
    let fnSuccess; // 成功時執行    

    that.init = function (fnData) {
        fnData = fnData || null;

        // 鎖定&&解鎖
        function Locking(type) {
            $('button[data-event="lock"]').prop('disabled', type);
        }

        // 新增&&編輯
        $('button[data-event="lock"]').off('click').click(function () {
            // 鎖定按鍵
            Locking(true);

            let myForm = $('#ActivityForm');
            let validator = myForm.validate();
            let formData = fnData != null ? fnData() : SerializeObject(myForm); // 資料繫結，擴充資料將由外部傳入
            myForm.off('submit').submit(function () {

                if ($(this).valid()) {
                    $.ajax({
                        url: this.action,
                        type: this.method,
                        data: formData,
                        async: false,
                        success: function (response) {

                            // Model 驗證錯誤
                            if (response.ModelStateErrors != null) {
                                validator.showErrors(response.ModelStateErrors);

                                //錯誤訊息顯示為紅色
                                $('label.error').addClass('text-danger');
                                // 解除鎖定按鍵
                                Locking(false);
                            }

                            // 新增/編輯成功失敗
                            if (response.IsSuccess != null) {
                                if (response.IsSuccess) {
                                    // 關閉編輯視窗，秀出回傳訊息，重新查詢
                                    ClosedModal();
                                    alert(response.Message);

                                    //成功時執行
                                    if ($.isFunction(fnSuccess = fnSuccess || Select)) {
                                        fnSuccess();
                                    }
                                }
                                else { // 失敗
                                    // 解除鎖定按鍵
                                    Locking(false);
                                    alert(response.Message);
                                }
                            }


                        }, Fail: function (response) {
                            console.log(response);
                            // 解除鎖定按鍵
                            Locking(false);
                            ClosedModal();
                        }
                    });
                }
                return false;
            }).trigger('submit');

            // 解除鎖定按鍵
            Locking(false);
        });

    };

    return that;
}());


/***************** 功能 - 分頁 *****************/
// 傳入外層 div， id ，以及查詢fun
// 覆寫原生 PagedList 分頁功能
let pagination = (function () {
    let that = {};
    that.init = function (Id, Select) {
        $(Id + " .pagination > li > a").off('click').click(function () {
            event.preventDefault();
            let index = this.href.split("=")[1] || 1;  // 前往頁碼

            // 傳入查詢函式
            if ($.isFunction(Select)) {
                Select(index);
            }
        });
    };
    return that;
}());


/***************** 功能 - 欄位排序功能 *****************/
// 範例  <th data-order="屬性名稱">
// 並於使用頁初始化功能 
// 範例 ordersByColumn.init("OrderType","PageNumber", fnSelect);
let ordersByColumn = (function () {
    let that = {};
    that.init = function (_OrderType, _PageNumber, fnSelect) {

        let OrderType = _OrderType;
        let PageNumber = _PageNumber;

        $('th[data-order]').append('<span data-ordermark=""></span>'); // 初始化先將設有 data-attribute，th 加入昇降符號
        $('span[data-ordermark]').text(OrderType == "Asc" ? "▲" : "▼");
        $('span[data-ordermark]').attr('data-ordermark', OrderType);

        // 點擊後事件
        $('th[data-order]').off('click').on('click', function () {
            console.log(this.dataset.order);
            let _Asc = 'Asc';
            let _Desc = 'Desc';
            let orderName = this.dataset.order; // 排序屬性
            let orderType = $(this)[0].firstElementChild.dataset.ordermark == _Asc ? _Desc : _Asc; // 排序類別，點擊後返回相反的類別
            let DescHtml = '<span data-ordermark="Desc">▼</span>'; // 顯示符號
            let AscHtml = '<span data-ordermark="Asc">▲</span>';
            let markHtml = orderType == _Asc ? AscHtml : DescHtml; // 給予當前相反符號

            $('span[data-ordermark]').remove(); // 刪除當前符號
            $(this).append(markHtml); // 加入符號

            let sortOrder = {
                OrderType: orderType,
                OrderName: orderName,
            }

            // 傳入查詢函式
            if ($.isFunction(fnSelect)) {
                fnSelect(PageNumber, sortOrder);
            }

        });

    };

    return that;
}());