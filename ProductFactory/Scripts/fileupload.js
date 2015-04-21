$(document).on('change', '.btn-file :file', function () {
	var input = $(this),
		numFiles = input.get(0).files ? input.get(0).files.length : 1,
		label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
	input.trigger('fileselect', [numFiles, label]);
});

$(document).ready(function () {
	$('.btn-file :file').on('fileselect', function (event, numFiles, label) {

		var input = $(this).parents('.input-group').find(':text'),
            log = numFiles > 1 ? numFiles + ' files selected' : label;

		if (input.length) {
			input.val(log);
		} else {
			if (log) alert(log);
		}

	});
	$("#upload").click(function (evt) {
		var files = $("#file1").get(0).files;
		if (files.length > 0) {
			var data = new FormData();
			for (i = 0; i < files.length; i++) {
				data.append("file" + i, files[i]);
			}
			$.ajax({
				type: "POST",
				url: "api/FileUploadApi",
				contentType: false,
				processData: false,
				data: data,
				success: function (messages) {
					for (i = 0; i < messages.length; i++) {
						console.log(messages[i]);
					}
				},
				error: function () {
					console.log("Error while invoking the Web API");
				}
			});
		}
	});
});