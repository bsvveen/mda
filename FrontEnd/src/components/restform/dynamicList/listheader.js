import React, { Component } from "react";
import PropTypes from 'prop-types';

const HeaderRow = (props) => {
  return Object.keys(props.sampleRecord).map((key) => {
    if (key !== "id" && !key.includes("_id"))
      return <th className="cell" key={key}><span onClick={() => props.onColumnClick(key)}>{key}</span></th>;
    return null;
  });
}

export default class ListHeader extends Component {

  PropTypes = {    
    sampleRecord : PropTypes.object.isRequired,
    onColumnClick: PropTypes.func.isRequired    
  }   

  onHeaderColumnClick = (key) => {
    this.props.onColumnClick(key);
  }   

  render() {   
    return (
      <tr>               
        <HeaderRow sampleRecord={this.props.sampleRecord} onColumnClick={this.onHeaderColumnClick} />           
      </tr>
    );  
  }
}