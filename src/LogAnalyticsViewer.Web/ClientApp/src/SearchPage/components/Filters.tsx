import { Button, Menu, MenuItem, Popover } from "@blueprintjs/core";
import { DateInput, IDateFormatProps } from "@blueprintjs/datetime";
import { CARET_DOWN } from "@blueprintjs/icons/lib/esm/generated/iconNames";
import React, { useReducer, useState } from "react";
import moment from "moment";
import { filterActions, filtersReducer } from "../state/filtersReducer";

export const Filters: React.FC = () => {
    const [state, dispatch] = useReducer(filtersReducer, { messageFilters: {} });
    const [date, setDate] = useState(null as Date);
    return (
        <div>
            <DateFilterInput value={state.from} onChange={value => dispatch(filterActions.setFrom(value))} />
            <DateFilterInput value={date} onChange={setDate} />
            <AddMessageFilter />
            <div>{moment(state.from).format("YYYY-MM-DD HH:mm:ss")}</div>
        </div>
    );
};



function momentFormatter(format: string): IDateFormatProps {
    return {
        formatDate: date => moment(date).format(format),
        parseDate: str => moment(str, format).toDate(),
        placeholder: `${format}`,
    };
}

const format =  momentFormatter("YYYY-MM-DD HH:mm:ss");

type DateFilterInputProps = Pick<React.ComponentProps<typeof DateInput>, "value" | "onChange">;
const DateFilterInput: React.FC<DateFilterInputProps> = (props) => {
    return <DateInput {...format} timePrecision={"second"} {...props} />
};


const AddMessageFilter: React.FC = () => {
    return (
        <Popover>
            <Button rightIcon={CARET_DOWN} text={"Add message filter"} />
            <Menu>
                <MenuItem text={"Like"} onClick={() => console.log("Add like")} />
                <MenuItem text={"Not Like"} onClick={() => console.log("Add not like")} />
            </Menu>
        </Popover>
    )
};
