import React from "react";
import Input from "../common/Input";
import SelectInput from "../common/SelectInput";
import PropTypes from "prop-types";

const RregisterRestaurantForm = ({ restaurant, towns, onChange, onSubmit }) => {
    const handleSubmit = event => {
      event.preventDefault();
      onSubmit(tab);
    };
  
    return (
      <>
        <h4>Register a new restaurant</h4>
        <form className="form-group" onSubmit={handleSubmit}>
          <Input
            name="restaurantName"
            value={restaurant.name}
            label="Restaurant name"
            onChange={onChange}
            placeholder="Restaurant name..."
          />
          <SelectInput
            options={towns.map(town => {
              return {
                value: table.number,
                text: table.number
              };
            })}
            defaultOption="Select table..."
            name="tableNumber"
            value={tab.tableNumber}
            label="Table number"
            onChange={onChange}
          />
          <input type="submit" className="btn btn-success" value="Open tab" />
        </form>
      </>
    );
  };

OpenTabForm.propTypes = {
    tab: PropTypes.object.isRequired,
    tables: PropTypes.array.isRequired,
    onChange: PropTypes.func.isRequired,
    onSubmit: PropTypes.func.isRequired
  };
  
  export default OpenTabForm;