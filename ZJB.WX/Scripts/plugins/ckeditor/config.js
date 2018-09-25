/*
Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
    



    //使用的工具栏 plugins/toolbar/plugin.js
    config.toolbar = 'Full';
    

    config.toolbar_Full =
    [
        ['Source'], ['Font', 'FontSize'],
     ['TextColor', 'BGColor'],
        ['Paste', 'PasteText', 'PasteFromWord' ],
        ['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
      ['Maximize']
    ];

    //工具栏是否可以被收缩 plugins/toolbar/plugin.js.
    config.toolbarCanCollapse = true;
    config.height = 300;
    //工具栏的位置 plugins/toolbar/plugin.js
    config.toolbarLocation = 'top';//可选：bottom

    //工具栏默认是否展开 plugins/toolbar/plugin.js
    config.toolbarStartupExpanded = true;

    //撤销的记录步数 plugins/undo/plugin.js
    config.undoStackSize = 20;
    
    config.forcePasteAsPlainText = false; //不去除
    //当从word里复制文字进来时，是否进行文字的格式化去除 plugins/pastefromword/plugin.js
    config.pasteFromWordIgnoreFontFace = false; //默认为忽略格式

    //是否使用 等标签修饰或者代替从word文档中粘贴过来的内容 plugins/pastefromword/plugin.js
    config.pasteFromWordKeepsStructure = false;

    //从word中粘贴内容时是否移除格式 plugins/pastefromword/plugin.js
    config.pasteFromWordRemoveStyle = false;
};

