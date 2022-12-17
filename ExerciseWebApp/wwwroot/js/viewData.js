document.addEventListener('DOMContentLoaded', () => {
    // Add event listeners for click events on the table header cells
    const headerCells = document.querySelectorAll('th');
    headerCells.forEach(function (cell) {
        cell.addEventListener('click', function (event) {
            // Get the name of the property to sort by
            const sortProperty = event.target.getAttribute('data-sort');

            // Get the current sorting preference
            let sortPreference = event.target.getAttribute('data-sort-preference') || 'unsorted';

            // Set the new sorting preference
            event.target.classList = [];
            if (sortPreference === 'asc') {
                event.target.setAttribute('data-sort-preference', 'desc');
                event.target.classList.add('desc');
            } else if (sortPreference === 'desc') {
                event.target.setAttribute('data-sort-preference', 'unsorted');
                event.target.classList.add('unsorted');
            } else {
                event.target.setAttribute('data-sort-preference', 'asc');
                event.target.classList.add('asc');
            }
            sortPreference = event.target.getAttribute('data-sort-preference');

            const sortCriteria = [`${sortProperty} ${sortPreference}`];

            // Send an AJAX request to the server to retrieve the sorted data
            $.ajax({
                type: 'POST',
                url: '/SortData',
                data: { sortCriteria: sortCriteria },
                success: function (data) {
                    updateTable(data);
                }
            });
        });
    });
});

function updateTable(data) {
    // Clear thecurrent data from the table
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
