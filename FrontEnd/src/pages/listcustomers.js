import React from 'react';
import { DashBoard, Tile } from '../components/dashboard';
import ListAndNew from '../components/restform/listandnew';

 {/*  <List entity="Customers" properties={["Name"]} constrains={[{Property : "Name", Operator: 0, Value: "Mirjam"}]} /> */}

export default  function ListCustomers(model) {   
    return (       
        <DashBoard>
            <Tile id="1" pane="left" title="Customers" >
                <ListAndNew entity="Customers" properties={["Name","Number", "BirthDate", "Comment"]} />           
            </Tile> 
            <Tile id="2" pane="right" title="Products" >           
                <ListAndNew entity="Products" properties={["Name","Number"]} />
            </Tile>      
        </DashBoard> 
    );
};