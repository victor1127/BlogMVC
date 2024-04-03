let index = 0;

function AddTag() {
    var tagEntry = document.getElementById("TagEntry");
    //create a new select option
    let newOption = new Option(tagEntry.value, tagEntry.value);
    document.getElementById("TagList").Options[index++] = newOption;
    //clear the tag entr
    tagEntry.value = "";
    return true;
}

function RemoveTag() {
    var selectedIndex = document.getElementById("TagList").selectedIndex;
    var selectedTag = document.getElementById("TagList").Options[selectedIndex];
    document.getElementById("TagList").remove(selectedTag);


}