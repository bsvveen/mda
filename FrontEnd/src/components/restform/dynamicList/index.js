import React, { Component } from "react";
import PropTypes from 'prop-types';

import ListHeader from './listheader';

export default class DynamicList extends Component {

  PropTypes = {
    items: PropTypes.array.isRequired,
    rowRender: PropTypes.func.isRequired,
    onSelect: PropTypes.func
  }

  state = { sortOn: undefined, sortAsc: true, searchFor: undefined, selectedItem: undefined };

  onSortClick = (sortOn) => {
    this.setState({ sortOn: sortOn, sortAsc: !this.state.sortAsc });
  }

  sortCallback = (sortOn, sortAsc) => {
    var sortOrder = sortAsc ? 1 : -1;   
    return function (a, b) {
      var result = (a[sortOn] < b[sortOn]) ? -1 : (a[sortOn] > b[sortOn]) ? 1 : 0;
      return result * sortOrder;
    }
  }

  onSearchChange = (e) => {
    if (e.target.value.length <= 3)
    this.setState({ searchFor: undefined });

    if (e.target.value.length > 3)
      this.setState({ searchFor: e.target.value });
  }

  filterCallback = (userInput) => {  
    return function (obj) {
      if (userInput === undefined)
        return true;

      let matchValue = userInput.toLowerCase();     

      return Object.values(obj).reduce((result, pV) => {
        return result || (typeof pV === 'string' && pV.toLowerCase().indexOf(matchValue) !== -1)
      }, false);
    }
  }

  onSelect = (item) => {
    this.setState({ selectedItem: item });

    if (this.props.onSelect)
      this.props.onSelect(item);
  }

  renderList = (items) => {
    return (items
      .filter(this.filterCallback(this.state.searchFor))
      .sort(this.sortCallback(this.state.sortOn, this.state.sortAsc))
      .map((item) => { return this.rowRender(item) })
      )
  }

  rowRender = (item) => {
    let isSelected = this.state.selectedItem && this.state.selectedItem.id === item.id;

    if (this.props.rowRender)
      return this.props.rowRender(item, isSelected);

    return (
      <tr key={item.Id} onClick={() => this.onSelect(item)} className={(isSelected) ? "selected" : null}>
        {Object.keys(item)
          .filter((key) => { return (key !== "ID" && !key.includes("_ID")) })
          .map((key) => {
            return <td key={key}>{item[key]}</td>
          })}
      </tr>)
  }

  render() {
    if (!this.props.items || this.props.items.length === 0)
      return <div>Geen informatie</div>;

    return (
      <div className="table">        
        <input type="search" onChange={this.onSearchChange} placeholder="zoeken binnen de lijst..."></input>
        <table><tbody>
          <ListHeader sampleRecord={this.props.items[0]} onColumnClick={this.onSortClick} />
          {this.renderList(this.props.items)}
        </tbody></table>
      </div>
    );
  }
}