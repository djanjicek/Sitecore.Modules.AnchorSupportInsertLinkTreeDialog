define([], function () {
    var action = function (context, args) {
        var targetDisplayTextID = context.app[args.targetDisplayTextID],
          targetPathID = context.app[args.targetPathID],
          targetPathProperty = args.targetPathProperty,
          targetQueryID = context.app[args.targetQueryID],
          targetAltTextID = context.app[args.targetAltTextID],
          targetStyleID = context.app[args.targetStyleID],
          targetWindowID = context.app[args.targetWindowID],
          customUrlID = context.app[args.customUrlID],

          // Define a variable called targetAnchorID (this name we used in our rule definition) and 
          // assign it the value of context.app[args.targetAnchorID]
          targetAnchorID = context.app[args.targetAnchorID],

          targetControlID = context.app[args.targetControlID],
            selectedItemsPropertyName = "selectedNode",

          // Extend the value of the template variable with an anchor attribute
          template = '<link anchor="<%= anchor %>" text="<%=displayText%>" linktype="internal" class="<%=styleClass%>" title="<%=alternateText%>" <%=target%> querystring="<%=queryString%>" id="<%=itemId%>" />',

          targetWindowValue, itemLink, path,
          emptyOptionID = "{A3C9DB39-1D1B-4AA1-8C68-7B9674D055EE}",
          htmlEncode = function (str) {
              return str.toString().replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
          };

        if (!targetDisplayTextID) {
            console.log("Provide at least display text for your link");
            return false;
        }

        targetWindowValue = targetWindowID.get("selectedItem");

        if (!targetWindowValue || targetWindowValue.itemId === emptyOptionID) {
            targetWindowValue = "";
        } else {
            var targetWindow = targetWindowValue.$displayName.trim();

            switch (targetWindow) {
                case 'Active Browser':
                    targetWindow = "";
                    break;
                case 'New Browser':
                    targetWindow = "_blank";
                    break;
                case "Custom":
                    targetWindow = customUrlID.get("text");
                    break;
                default:
                    targetWindow = "";
            }

            targetWindowValue = "target=\"" + targetWindow + "\"";
        }

        if (targetPathID.get(selectedItemsPropertyName) &&
              "rawItem" in targetPathID.get(selectedItemsPropertyName) &&
              targetPathID.get(selectedItemsPropertyName).rawItem[targetPathProperty]) {
            path = targetPathID.get(selectedItemsPropertyName).rawItem[targetPathProperty];
        }

        itemLink = _.template(template, {
            displayText: htmlEncode(targetDisplayTextID.get("text")),
            alternateText: htmlEncode(targetAltTextID.get("text")),
            itemId: targetControlID.get("selectedItemId"),
            queryString: htmlEncode(targetQueryID.get("text")),
            target: targetWindowValue,
            styleClass: targetStyleID.get("text"),
            path: path,

            // Store the anchor value retrieved from the dialog in the anchor attribute
            anchor: targetAnchorID.get("text")
        });

        context.app.closeDialog(itemLink);
    };

    return action;
});
