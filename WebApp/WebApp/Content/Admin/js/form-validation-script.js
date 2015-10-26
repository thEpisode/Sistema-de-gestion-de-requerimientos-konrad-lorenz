var Script = function () {

   

    $().ready(function() {
        // validate the comment form when it is submitted
        $("#create_agent_form").validate();

        /*
         * Translated default messages for the jQuery validation plugin.
         * Locale: ES
         */
        jQuery.extend(jQuery.validator.messages, {
            required: "Este campo es obligatorio.",
            remote: "Por favor, rellena este campo.",
            email: "Por favor, escribe una direcci�n de correo v�lida",
            url: "Por favor, escribe una URL v�lida.",
            date: "Por favor, escribe una fecha v�lida.",
            dateISO: "Por favor, escribe una fecha (ISO) v�lida.",
            number: "Por favor, escribe un n�mero entero v�lido.",
            digits: "Por favor, escribe s�lo d�gitos.",
            creditcard: "Por favor, escribe un n�mero de tarjeta v�lido.",
            equalTo: "Por favor, escribe el mismo valor de nuevo.",
            accept: "Por favor, escribe un valor con una extensi�n aceptada.",
            maxlength: jQuery.validator.format("Por favor, no escribas m�s de {0} caracteres."),
            minlength: jQuery.validator.format("Por favor, no escribas menos de {0} caracteres."),
            rangelength: jQuery.validator.format("Por favor, escribe un valor entre {0} y {1} caracteres."),
            range: jQuery.validator.format("Por favor, escribe un valor entre {0} y {1}."),
            max: jQuery.validator.format("Por favor, escribe un valor menor o igual a {0}."),
            min: jQuery.validator.format("Por favor, escribe un valor mayor o igual a {0}.")
        });

    });


}();