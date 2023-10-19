// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var searchInput = document.getElementById("searchInput");
searchInput.addEventListener("keyup", () => {
	

		// Creating Our XMLHttpRequest object 
		let xhr = new XMLHttpRequest();

		// Making our connection 
	let url = `https://localhost:44348/Employee/Index?searcInput=${searchInput.value}`;
		xhr.open("POST", url, true);

		// function execute after request is successful 
		xhr.onreadystatechange = function () {
			if (this.readyState == 4 && this.status == 200) {
				console.log(this.response);
			}
		}
		// Sending our request 
		xhr.send();


})