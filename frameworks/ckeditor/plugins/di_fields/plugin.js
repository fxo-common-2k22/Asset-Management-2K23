(function () {
    var addField = {
        canUndo: false,
        exec: function(editor){
            var span = editor.document.createElement('span');
//            span.innerHTML = 'hello';
            editor.insertText('hello');
        }
    };

    var pluginName = 'di_fields';
    CKEDITOR.plugins.add('di_fields', {
        lang: 'en',
        icons: 'bold',
        hidpi: true,
        init: function (editor) {
            console.log('ini');
            if (editor.blockless)
                return;
            //var order = 0;
            editor.addCommand(pluginName, addField);
            editor.ui.addButton && editor.ui.addButton('FatherName', {
                label: 'Father Name',
                command: 'di_fields',
                icon: 'bold'
            });
        }
    });
})();