import React, { Component } from "react";
import PropTypes from 'prop-types';

class ItemProps extends Component {   

  PropTypes = {   
    created: PropTypes.string, 
    createdby: PropTypes.string,
    created: PropTypes.string,
    createdby: PropTypes.string,   
  }  

  formatDate(d) {  
    const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };       
    return new Date(d).toUTCString();
  }

  render() { 
    if (!this.props.created)
        return null;

    return (
        <div className="small">
            <div>Gecreeerd: { this.formatDate(this.props.created) } door { this.props.createdby }</div>
            <div>Aangepast: { this.formatDate(this.props.modified) } door { this.props.modifiedby }</div>
        </div>
    ) }
}

export default ItemProps;