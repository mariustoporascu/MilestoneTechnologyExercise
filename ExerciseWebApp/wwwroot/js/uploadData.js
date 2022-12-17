document.addEventListener('DOMContentLoaded', () => {
	const fileListContainer = document.getElementById('file-list');
	const addFilesButton = document.getElementById('add-files-button');

	// Get the file item template
	const fileItemTemplate = document.getElementById('file-item-template');

	// Handle adding files to the file list
	addFilesButton.addEventListener('click', () => {
		const filesInput = document.getElementById('files');
		const files = Array.from(filesInput.files);
		if (files.length > 0) {
			files.forEach((file, index) => {
				// Clone the file item template
				const fileItem = fileItemTemplate.content.cloneNode(true);

				// Set the file name
				fileItem.querySelector('.file-name').textContent = file.name;

				// Set the delimiter select name
				fileItem.querySelector('.delimiter-select').name = `delimiter-${index}`;
				fileItem.querySelector('.delimiter-select').id = `delimiter-${index}`;

				// Add a data attribute to the file item with the index
				fileItem.querySelector('.file-item').dataset.index = index;

				fileListContainer.appendChild(fileItem);

				// Remove the call for this function if you want configurate input manually
				// This is for debugging purposes only
				populateDebugData(file.name,index);
			});

			addFilesButton.style.display = 'none';
			const filesDiv = document.getElementById('filesSelectDiv');
			const submitButton = document.getElementById('submit-button');
			filesDiv.style.display = 'none';
			submitButton.style.display = 'inline-block';
		} else {
			alert('No file selected.')
        }
	});
	// Handle adding headers to the header list for each file
	fileListContainer.addEventListener('click', (event) => {
		if (event.target.classList.contains('add-header')) {
			const fileItem = event.target.parentNode;
			const headerList = fileItem.querySelector('.header-list');

			const headerItem = document.createElement('div');
			headerItem.classList.add('header-item');

			const headerNameSelect = document.createElement('select');
			headerNameSelect.classList.add('header-name-select');
			headerNameSelect.name = `header-name-${fileItem.dataset.index}`;
			headerNameSelect.innerHTML = `
								<option value="CompanyName">Company Name</option>
								<option value="YearsInBusiness">Years In Business</option>
								<option value="YearFounded">Year Founded</option>
								<option value="FullName">Contact Full Name</option>
								<option value="FirstName">Contact First Name</option>
								<option value="LastName">Contact Last Name</option>
								<option value="PhoneNumber">Contact Phone Number</option>
								<option value="Email">Contact Email</option>
							`;
			headerItem.appendChild(headerNameSelect);

			const headerOrderInput = document.createElement('input');
			headerOrderInput.classList.add('header-order-input');
			headerOrderInput.name = `header-order-${fileItem.dataset.index}`;
			headerOrderInput.type = 'number';
			headerOrderInput.min = '1';
			headerOrderInput.value = headerList.childNodes.length + 1;
			headerItem.appendChild(headerOrderInput);

			const moveUpButton = document.createElement('button');
			moveUpButton.classList.add('arrow-button', 'move-up-button');
			moveUpButton.setAttribute('type', 'button');
			moveUpButton.setAttribute('title', 'Move Up');
			moveUpButton.innerHTML = '&uarr;';
			headerItem.appendChild(moveUpButton);

			const moveDownButton = document.createElement('button');
			moveDownButton.classList.add('arrow-button', 'move-down-button');
			moveDownButton.setAttribute('type', 'button');
			moveDownButton.setAttribute('title', 'Move Down');
			moveDownButton.innerHTML = '&darr;';
			headerItem.appendChild(moveDownButton);

			const deleteButton = document.createElement('button');
			deleteButton.classList.add('delete-button');
			deleteButton.setAttribute('type', 'button');
			deleteButton.setAttribute('title', 'Remove');
			deleteButton.innerHTML = '&times;';
			headerItem.appendChild(deleteButton);

			headerList.appendChild(headerItem);
		}
	});
	// Handle moving headers up and down in the header list for each file
	fileListContainer.addEventListener('click', (event) => {
		if (event.target.classList.contains('move-up-button')) {
			const headerItem = event.target.parentNode;
			const headerList = headerItem.parentNode;
			const previousHeader = headerItem.previousSibling;
			const headerItemIndex = Array.from(headerList.childNodes).indexOf(headerItem);
			const previousHeaderIndex = Array.from(headerList.childNodes).indexOf(previousHeader);
			if (previousHeader) {
				headerItem.querySelector('.header-order-input').value = previousHeaderIndex + 1;
				previousHeader.querySelector('.header-order-input').value = headerItemIndex + 1;
				headerList.insertBefore(headerItem, previousHeader);
			}
		} else if (event.target.classList.contains('move-down-button')) {
			const headerItem = event.target.parentNode;
			const headerList = headerItem.parentNode;
			const nextHeader = headerItem.nextSibling;
			const headerItemIndex = Array.from(headerList.childNodes).indexOf(headerItem);
			const nextHeaderIndex = Array.from(headerList.childNodes).indexOf(nextHeader);
			if (nextHeader) {
				headerItem.querySelector('.header-order-input').value = nextHeaderIndex + 1;
				nextHeader.querySelector('.header-order-input').value = headerItemIndex + 1;
				headerList.insertBefore(nextHeader, headerItem);
			}
		}
	});

	// Handle deleting headers from the header list for each file
	fileListContainer.addEventListener('click', (event) => {
		if (event.target.classList.contains('delete-button')) {
			const headerItem = event.target.parentNode;
			const headerList = headerItem.parentNode;
			headerList.removeChild(headerItem);

			// Update the order of the remaining headers
			const headerItems = headerList.querySelectorAll('.header-item');
			headerItems.forEach((item, index) => {
				item.querySelector('.header-order-input').value = index + 1;
			});
		}
	});

});

function populateDebugData(fileName, index) {
	const delimiterSelect = document.getElementById(`delimiter-${index}`);
	const fileDelimiter = delimiterSelect.parentNode;
	const fileItem = fileDelimiter.parentNode;
	const addHeaderButton = fileItem.querySelector('.add-header');

	switch (fileName) {
		case 'comma.txt':;
			delimiterSelect.value = ',';
			for (let i = 0; i < 5; i++) {
				addHeaderButton.click();
				const headerItems = fileItem.querySelectorAll('.header-item');
				const headerNameSelect = headerItems[i].querySelector('.header-name-select');
				if (i === 0)
					headerNameSelect.value = 'CompanyName';
				else if (i === 1)
					headerNameSelect.value = 'FullName';
				else if (i === 2)
					headerNameSelect.value = 'PhoneNumber';
				else if (i === 3)
					headerNameSelect.value = 'YearsInBusiness';
				else
					headerNameSelect.value = 'Email';
            }
			break;
		case 'hash.txt':
			delimiterSelect.value = '#';
			for (let i = 0; i < 4; i++) {
				addHeaderButton.click();
				const headerItems = fileItem.querySelectorAll('.header-item');
				const headerNameSelect = headerItems[i].querySelector('.header-name-select');
				if (i === 0)
					headerNameSelect.value = 'CompanyName';
				else if (i === 1)
					headerNameSelect.value = 'YearFounded';
				else if (i === 2)
					headerNameSelect.value = 'FullName';
				else
					headerNameSelect.value = 'PhoneNumber';
			}
			break;
		case 'hyphen.txt':
			delimiterSelect.value = '-';
			for (let i = 0; i < 6; i++) {
				addHeaderButton.click();
				const headerItems = fileItem.querySelectorAll('.header-item');
				const headerNameSelect = headerItems[i].querySelector('.header-name-select');
				if (i === 0)
					headerNameSelect.value = 'CompanyName';
				else if (i === 1)
					headerNameSelect.value = 'YearFounded';
				else if (i === 2)
					headerNameSelect.value = 'PhoneNumber';
				else if (i === 3)
					headerNameSelect.value = 'Email';
				else if (i === 4)
					headerNameSelect.value = 'FirstName';
				else
					headerNameSelect.value = 'LastName';
			}
			break;
		default:
			break;
    }
}