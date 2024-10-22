function validar(valor) {
patron =/[ÁáÉéÍíÓóÚúÀàÈèÌìÒòÙù]/;
if (patron.test(valor))
alert('Error caracteres no permitidos');
}