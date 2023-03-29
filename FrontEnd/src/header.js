import React from 'react';
import { Link } from "react-router-dom";

export default class Header extends React.Component {
  render() {
    return (
      <div id="header" className="tile">
        <div>
          <div className="logo" alt="logo" />
          <div className="menu">            
            <Link to="/" title="Traject zoeken"><i className="button fas fa-search fa-2x"></i></Link>
            <Link to="/beheergebruikers/" title="Beheer Gebruikers"><i className="button fas fa-users fa-2x"></i></Link>
            <Link to="/nieuweclienttoevoegen/" title="Nieuwe client toevoegen"><i className="button fas fa-plus fa-2x"></i></Link>
            <Link to="/nieuweorganisatietoevoegen/" title="Nieuwe organisatie toevoegen"><i className="button fas fa-plus fa-2x"></i></Link>
            <Link to="/organisatiesbeheren/" title="Organisaties beheren"><i className="button fas fa-building fa-2x"></i></Link>  
          </div>              
        </div>
      </div>
    );
  }
}