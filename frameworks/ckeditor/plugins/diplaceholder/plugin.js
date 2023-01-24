/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

/**
 * @fileOverview The "diplaceholder" plugin.
 *
 */

'use strict';

( function() {
	CKEDITOR.plugins.add( 'diplaceholder', {
		requires: 'widget,dialog',
		lang: 'en', // %REMOVE_LINE_CORE%
		icons: 'diplaceholder', // %REMOVE_LINE_CORE%
		hidpi: true, // %REMOVE_LINE_CORE%

		onLoad: function() {
			// Register styles for placeholder widget frame.
			CKEDITOR.addCss( '.cke_diplaceholder{background-color:#ff0}' );
		},

		init: function( editor ) {

			var lang = editor.lang.placeholder;

			// Register dialog.
			CKEDITOR.dialog.add( 'diplaceholder', this.path + 'dialogs/diplaceholder.js' );

			// Put ur init code here.
			editor.widgets.add( 'diplaceholder', {
				// Widget code.
				dialog: 'diplaceholder',
				pathName: lang.pathName,
				// We need to have wrapping element, otherwise there are issues in
				// add dialog.
				template: '<span class="cke_diplaceholder">{}</span>',

				downcast: function() {
					return new CKEDITOR.htmlParser.text( '{' + this.data.name + '}' );
				},

				init: function() {
					// Note that placeholder markup characters are stripped for the name.
					this.setData( 'name', this.element.getText().slice( 1, -1 ) );
				},

				data: function() {
					this.element.setText( '{' + this.data.name + '}' );
				}
			} );

			editor.ui.addButton && editor.ui.addButton( 'CreatePlaceholder', {
				label: lang.toolbar,
				command: 'diplaceholder',
				toolbar: 'insert,5',
				icon: 'diplaceholder'
			} );
		},

		afterInit: function( editor ) {
			var placeholderReplaceRegex = /\{([^[\]])+\}/g;

			editor.dataProcessor.dataFilter.addRules( {
				text: function( text, node ) {
					var dtd = node.parent && CKEDITOR.dtd[ node.parent.name ];

					// Skip the case when placeholder is in elements like <title> or <textarea>
					// but upcast placeholder in custom elements (no DTD).
					if ( dtd && !dtd.span )
						return;

					return text.replace( placeholderReplaceRegex, function( match ) {
						// Creating widget code.
						var widgetWrapper = null,
							innerElement = new CKEDITOR.htmlParser.element( 'span', {
								'class': 'cke_diplaceholder'
							} );

						// Adds placeholder identifier as innertext.
						innerElement.add( new CKEDITOR.htmlParser.text( match ) );
						widgetWrapper = editor.widgets.wrapElement( innerElement, 'diplaceholder' );

						// Return outerhtml of widget wrapper so it will be placed
						// as replacement.
						return widgetWrapper.getOuterHtml();
					} );
				}
			} );
		}
	} );

} )();
