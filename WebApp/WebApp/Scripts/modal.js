function showModal(title, text, footer) {
	$('.tour-tour').remove();
    if (title) $('#modalTitle').html(title)
    else $('#modalTitle').html('')
    if (text) $('#modalMessage').html(text)
    else $('#modalMessage').html('')
    if (footer) $('#modalFooter').html(footer)
    else $('#modalFooter').html('')
    $('#modalAlert').modal()
}

function hideModal() {
    $('#modalAlert').modal('hide');
}
$('.modal').on('shown.bs.modal', function () {
    $(this).find('[autofocus]').focus();
});
$('.modal').on('show.bs.modal', function () {
    $(this).find('[autofocus]').focus();
});