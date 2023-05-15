import React, { Component } from "react";
import PropTypes from 'prop-types';

import Repository from './repository';
import List from "./list"
import Form from "./form"
import Detail from "./detail"

export default class AdvancedList extends Component {

  PropTypes = {
    entity: PropTypes.string.isRequired,
    properties: PropTypes.array,    
    constrains: PropTypes.object    
  }

  state = { current: undefined, mode: 'list' };

  componentDidMount() { 
    this.repository = new Repository(this.props.entity); 
  }   

  rowRender = (item, isSelected) => {   
    return (
      <tr onClick={() => this.onSelect(item, "detail")} key={item.id} className={(isSelected) ? "selected" : null}> 
        {
          Object.keys(item)
          .filter((key) => { return (key !== "id" && !key.includes("_id"))})
          .map((key) => { return <td key={key}>{item[key]}</td>})
        } 
        <td className="small"><button onClick={() => this.onSelect(item, "form")} type="edit" title="Aanpassen" /></td>               
      </tr>);
  }

  onSelect = (item, mode) => {
    this.repository.GetById(item.Id).then(response => {      
      this.setState({ current: response, mode: mode })
    });    
  } 

  onNew = () => { this.setState({ current: {} }); } 

  onReset = () => { this.setState({ current: undefined }); }  
 
  render() {    
    if (this.state.current && this.state.mode == "form")
      return <Form  entityName = { this.props.entity }
                    data = { this.state.current }       
                    constrains = { this.state.constrains }
                    onCancel = {this.onReset}
                    onSubmit = {this.onReset}
                    onDelete = {this.onReset} />

    if (this.state.current && this.state.mode == "detail")
      return <Detail  entityName = { this.props.entity }
                      data = { this.state.current }  
                      onCancel = {this.onReset} />

    return (
      <div>
        <List entity={this.props.entity} rowRender={this.rowRender} constrains={this.props.constrains} />
        <div className="actions"><button type="new" onClick={this.onNew} value="Nieuw" /></div>
      </div>
      )     
  }
}