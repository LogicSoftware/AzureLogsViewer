import { DateInput, IDateFormatProps } from "@blueprintjs/datetime";
import moment from "moment";
import React from "react";

function momentFormatter(format: string): IDateFormatProps {
    return {
        formatDate: date => moment(date).format(format),
        parseDate: str => moment(str, format).toDate(),
        placeholder: `${format}`,
    };
}

const format = momentFormatter("YYYY-MM-DD HH:mm:ss");
type DateFilterInputProps = Pick<React.ComponentProps<typeof DateInput>, "value" | "onChange">;
export const DateFilterInput: React.FC<DateFilterInputProps> = (props) => {
    return <DateInput {...format} timePrecision={"second"} {...props} />
};