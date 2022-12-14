import React, { Component } from "react";
import PropTypes from 'prop-types';

import Repository from './repository';
import List from "./list"
import Form from "./form"

export default class ListDetail extends Component {

  PropTypes = {
    controller: PropTypes.string.isRequired,
    onSelect: PropTypes.func
  }

  state = { current: undefined };

  componentDidMount() { this.repository = new Repository(this.props.controller); }   

  rowRender = (item, isSelected) => {   
    return (
      <tr onClick={() => this.onListSelect(item)} key={item.id} className={(isSelected) ? "selected" : null}> 
        {
          Object.keys(item)
          .filter((key) => { return (key !== "id" && !key.includes("_id"))})
          .map((key) => { return <td key={key}>{item[key]}</td>})
        } 
        <td className="small"><button onClick={() => this.onEdit(item)} type="edit" title="Aanpassen" /></td>               
      </tr>);
  }

  onEdit = (item) => {
    this.repository.Get(item.id).then(response => {      
      this.setState({ current: response })
    });    
  } 

  onNew = () => { this.setState({ current: {} }); } 

  onFormCancel= () => { this.setState({ current: undefined }); }
  
  onFormSubmit = () => { this.setState({ current: undefined }); }

  onFormDelete = () => { this.setState({ current: undefined }); }

  onListSelect = (item) => { if (this.props.onSelect) this.props.onSelect(item); }

  render() {    
    if (this.state.current)
      return <Form data={ this.state.current } 
                  constrains = {this.props.constrains}
                  controller = {this.props.controller}
                  onCancel = {this.onFormCancel}
                  onSubmit = {this.onFormSubmit}  
                  onDelete = {this.onFormDelete} />

    return (
      <div>
        <List controller={this.props.controller} rowRender={this.rowRender} constrains={this.props.constrains} />
        <div className="actions"><button type="new" onClick={this.onNew} value="Nieuw" /></div>
      </div>
      )     
  }
}