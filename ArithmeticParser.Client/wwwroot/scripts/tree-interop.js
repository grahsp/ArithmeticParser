window.treeInterop = {
    render: function (elementId, data) {
        const svg = d3.select("#" + elementId);
        svg.selectAll("*").remove();

        const width = 600;
        const height = 400;

        const root = d3.hierarchy(data, d => d.children);

        const treeLayout = d3.tree().size([width /2, height /2])
        treeLayout(root);

        const g = svg
            .attr("width", width)
            .attr("height", height)
            .call(d3.zoom()
                .scaleExtent([1, 5])
                .on("zoom", function (event) {
                    g.attr("transform", event.transform);
                }))
            .append("g")

        // Links
        g.selectAll(".link")
            .data(root.links())
            .enter()
            .append("line")
            .attr("class", "link")
            .attr("x1", d => d.source.x)
            .attr("y1", d => d.source.y)
            .attr("x2", d => d.target.x)
            .attr("y2", d => d.target.y)
            .attr("stroke", "#ccc")
            .attr("thickness", 3);

        // Nodes
        const node = g.selectAll(".node")
            .data(root.descendants())
            .enter()
            .append("g")
            .attr("class", "node")
            .attr("transform", d => `translate(${d.x},${d.y})`);

        node.append("circle")
            .attr("r", 30)
            .attr("fill", "#a2ead7");

        node.append("text")
            .attr("dy", 4)
            .attr("text-anchor", "middle")
            .text(d => d.data.name);
    }
};