import React, { Component } from "react";
import PropTypes from 'prop-types';

import Repository from './repository';
import DynamicList from "./dynamicList"

class List extends Component {
  _isMounted = false;

  PropTypes = {
    entity: PropTypes.string.isRequired,
    properties: PropTypes.array,
    rowRender: PropTypes.func,
    constrains: PropTypes.object,
    onSelect: PropTypes.func
  }

  state = { isLoading: true, items: [] };

  componentDidMount() {  
    this._isMounted = true;  
    this.repository = new Repository(this.props.entity);       
    this.getList();     
  }  

  componentWillUnmount() {
    this._isMounted = false;
  }

  getList = () => {  
    this.setState({ isLoading: true });  
     
    this.repository.List(this.props.properties, this.props.constrains).then(response => {
      if (this._isMounted) {
      this.setState({ items: response }, () => {
        this.setState({ isLoading: false });
      })};
    })
  }    

  onListSelect = (item) => {
    if (this.props.onSelect)
        this.props.onSelect(item);       
  }

  render() {
    if (this.state.isLoading)
      return <div className="loader"></div>;    

    return (
      <div className="list">
        <DynamicList items={this.state.items} onSelect={this.onListSelect} rowRender={this.props.rowRender}  />
      </div>
    );
  }
}

export default List;