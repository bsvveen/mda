import React from 'react';
import List from '../components/restform/list'

export default  function ListCustomers(model) {   
    return (
        <div>
            <List entity="Customers" properties={["Name"]} constrains={
              { name: { "equals" : "Mirjam" }}} />
        </div>
    );
};