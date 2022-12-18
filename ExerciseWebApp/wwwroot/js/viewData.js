document.addEventListener('DOMContentLoaded', () => {
    // Reset saved prefs on page refresh
    localStorage.setItem('sortPrefs', JSON.stringify([]));
    // Add event listeners for click events on the table header cells
    const headerCells = document.querySelectorAll('th');
    headerCells.forEach( (cell) => {
        cell.addEventListener('click', (event) => {

            const sortProperty = event.target.getAttribute('data-sort');
            // Get the current sortPreference
            let sortPreference = event.target.getAttribute('data-sort-preference');

            const oldCellCriteria = `${sortProperty} ${sortPreference}`;

            const savedPrefs = getPreferences();
            let filteredPrefs = savedPrefs.filter((data) => {
                if (data !== oldCellCriteria) return data;
            });

            // Get next sortPreference
            sortPreference = getNextSort(sortPreference);

            
            let sortCriteriaArr = [];
            if (sortPreference !== 'unsorted')
                sortCriteriaArr = [...filteredPrefs, `${sortProperty} ${sortPreference}`];
            else
                sortCriteriaArr = [...filteredPrefs];

            // Keep only last 2 sorting prefs
            if (sortCriteriaArr.length === 3)
                sortCriteriaArr = sortCriteriaArr.splice(1, 2);

            // Save Preferences
            setPreferences(sortCriteriaArr);

            // Update the headers sorting class
            updateHeadersUI(sortCriteriaArr);

            // Send an AJAX request to the server to retrieve the sorted data
            $.ajax({
                type: 'POST',
                url: '/SortData',
                data: { sortCriteria: sortCriteriaArr },
                success: function (data) {
                    updateTable(data);
                }
            });
        });
    });
    document.querySelector('.export-button').addEventListener('click', () => {
        const sortingPreferences = getPreferences();
        // Construct the query string for the sorting preferences
        const queryString = `sortCriteria=${sortingPreferences.join(',')}`;
        // Send the request to the server with the sorting preferences in the query string
        const url = `/ExportData?${queryString}`;
        window.location = url;
    });
});

function updateHeadersUI(sortCriteriaArr) {
    const headerCells = document.querySelectorAll('th');
    headerCells.forEach((cell) => {
        const sortProperty = cell.getAttribute('data-sort');

        cell.classList = [];
        const hasPref = sortCriteriaArr.find((data) => {
            return data.split(' ')[0] === sortProperty;
        });

        if (hasPref) {
            const sortPreference = hasPref.split(' ')[1];
            cell.setAttribute('data-sort-preference', sortPreference);
            cell.classList.add(sortPreference);
        } else {
            cell.setAttribute('data-sort-preference', 'unsorted');
            cell.classList.add('unsorted');
        }
    });
}

function getNextSort(sortPreference) {
    if (sortPreference === 'asc') return 'desc';
    else if (sortPreference === 'desc') return 'unsorted';
    else return 'asc';
}

function setPreferences(sortCriteria) {
    const localSortPreference = JSON.stringify(sortCriteria);
    localStorage.setItem('sortPrefs', localSortPreference);
}

function getPreferences() {
    const localSortPreference = localStorage.getItem('sortPrefs');
    let sortPrefsArray = [];
    if (localSortPreference) {
        sortPrefsArray = JSON.parse(localSortPreference);
    }
    return sortPrefsArray;  
}

function updateTable(data) {
    // Clear the current data from the table
    let tableBody = document.querySelector('tbody');
    tableBody.innerHTML = '';
    // Populate the table with the new data
    data.forEach(function (company) {
        const row = document.createElement('tr');
        row.innerHTML = `<td data-sort-by="CompanyName">${company.companyName}</td> 
            <td data-sort-by="YearsInBusiness">${(company.yearsInBusiness ?? '')}</td> 
            <td data-sort-by="FullName">${(company.contactDetails.fullName ?? '')}</td> 
            <td data-sort-by="PhoneNumber">${(company.contactDetails.phoneNumber ?? '')}</td> 
            <td data-sort-by="Email">${(company.contactDetails.email ?? '')}</td>`;
        tableBody.appendChild(row);
    });
}
