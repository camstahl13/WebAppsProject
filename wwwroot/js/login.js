const inputs = document.querySelectorAll('.login-cred input');

function input_float(input) {
	if (input.value.length) {
		input.classList.add("filled");
	} else {
		input.classList.remove("filled");
	}
}

inputs.forEach(input => {
	input_float(input);
	input.addEventListener('blur', (event) => { input_float(event.target) });
})