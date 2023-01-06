import React, { Component } from 'react';
import PropTypes from 'prop-types';

import Repository from './repository';
import Form from './form';
import List from './list';

export default class ListAndNew extends Component {

  state = { current: undefined, refresh: true }

  PropTypes = {    
    entity: PropTypes.string.isRequired,
    properties: PropTypes.array,
    rowRender: PropTypes.func,
    constrains: PropTypes.object,
  }

  componentDidMount() {  this.repository = new Repository(this.props.entity); }    

  onFormSubmit = () => { this.setState({ current: undefined, refresh: !this.state.refresh })}

  onFormDelete = () => { this.setState({ current: undefined, refresh: !this.state.refresh })}

  onListSelect = (data) => {   
    this.repository.GetById(data.Id).then(response => {      
      this.setState({ current: response })
    });  
  }   

  render() {      
      return (<div className="listform">   
        <List {...this.props} onSelect={this.onListSelect} key={this.state.refresh} />       
        <hr />
        <h2 className="title">{(this.state.current) ? "Aanpassen" : "Nieuw" }</h2>
        <div>          
          <Form 
            key = { (this.state.current) ? this.state.current.Id : undefined }            
            data = { this.state.current }
            constrains = {this.props.constrains}
            entity = {this.props.entity}            
            onSubmit =  {this.onFormSubmit}
            onDelete = {this.onFormDelete}                   
          />
        </div>
      </div>)
    };
}