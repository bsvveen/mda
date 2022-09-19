import React from 'react';

function ListCustomers() {   
    return (
        <div>
            <List Entity="Customers" Properties={["Name"]} />
        </div>
    );
}

export default MyList;