jQuery.fn.setCursorPosition = function (position) {
    if (this.length == 0) return this;
    return $(this).setSelection(position, position);
}

jQuery.fn.setSelection = function (selectionStart, selectionEnd) {
    if (this.length == 0) return this;
    input = this[0];

    if (input.createTextRange) {
        var range = input.createTextRange();
        range.collapse(true);
        range.moveEnd('character', selectionEnd);
        range.moveStart('character', selectionStart);
        range.select();
    } else if (input.setSelectionRange) {
        input.focus();
        input.setSelectionRange(selectionStart, selectionEnd);
    }

    return this;
}

jQuery.fn.focusEnd = function () {
    this.setCursorPosition(this.val().length);
    return this;
}
jQuery.fn.setValueAutoFocus = function (v) {
    if (this.length == 0) return this;
    input = this[0];
    var me = this;
    if (document.selection) {
        me.focus();
        sel = document.selection.createRange();
        sel.text = v;
        sel.select();

    }
    //MOZILLA/NETSCAPE support
    else if (input.selectionStart || input.selectionStart == '0') {
        var startPos = input.selectionStart;
        var endPos = input.selectionEnd;
        myValue = v;
        var inputVal = me.val();
        me.val(inputVal.substring(0, startPos) + myValue + inputVal.substring(endPos, inputVal.length));
        input.selectionStart = startPos + myValue.length;
        input.selectionEnd = startPos + myValue.length;
    } else {
        input.value += myValue;
    }
    me.focus();
}
