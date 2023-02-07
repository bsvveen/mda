import React from 'react';
import List from '../components/restform/list';
import ListAndNew from '../components/restform/listandnew';

export default  function ListCustomers(model) {   
    return (
        <div>
            {/*  <List entity="Customers" properties={["Name"]} constrains={[{Property : "Name", Operator: 0, Value: "Mirjam"}]} /> */}
            <ListAndNew entity="Customers" properties={["Name","Number", "BirthDate", "Comment"]} />
            <ListAndNew entity="Products" properties={["Name","Number"]} />
        </div>
    );
};