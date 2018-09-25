/*
Copyright (c) 2003-2012, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';
	config.toolbar_Full = [
['Source','Font','FontSize','Bold','Italic','Underline','Strike','-','Subscript','Superscript'],
['NumberedList','BulletedList'],
['JustifyLeft','JustifyCenter','JustifyRight','JustifyBlock'],
['TextColor','BGColor'],
['Maximize']
]; 
	//config.width = 550; 
	config.height = 300; 
	config.enterMode = CKEDITOR.ENTER_BR;
	config.removePlugins = 'elementspath';
	config.resize_enabled = false;
//	config.defaultLanguage = "zh-cn";
//	config.font_defaultLabel = "宋体";
//	config.fontSize_defaultLabel = '48px'; 
	//config.skin = 'office2003';
	//config.skin = 'v2';
};
