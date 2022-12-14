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

  rowRender = (item) => {   
    return (
      <tr key={item.id}> 
        {
          Object.keys(item)
          .filter((key) => { return (key !== "id" && !key.includes("_id"))})
          .map((key) => { return <td key={key}>{item[key]}</td>})
        }                    
      </tr>);
  }

  getList = () => {  
    this.setState({ isLoading: true });  

    let constrains = this.getConstrainsFromProps(this.props);    
    this.repository.List(this.props.properties, constrains).then(response => {
      if (this._isMounted) {
      this.setState({ items: response }, () => {
        this.setState({ isLoading: false });
      })};
    })
  }  

  getConstrainsFromProps = (props) => {
    let filter = []
    if (props.constrains) {
      filter = Object.keys(props.constrains)
        .filter(key => props.constrains[key].equals)
        .reduce((obj, key) => {
          obj.push({ "field": key, "value": props.constrains[key].equals });
          return obj;
          }, []
        );   
    }
    return filter;
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
        <DynamicList items={this.state.items} rowRender={this.rowRender} onSelect={this.onListSelect}  />
      </div>
    );
  }
}

export default List;